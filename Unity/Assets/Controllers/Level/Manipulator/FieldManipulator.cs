using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Controllers.Level.Cursor;
using Engine.Geometry;
using Engine.Simulation;
using UnityEngine;

namespace Assets.Controllers.Level.Manipulator
{
    /// <summary>
    /// Manages user interactions with the simulation.
    /// </summary>
    public class FieldManipulator
    {
        private readonly IPolyhedron _polyhedron;
        private readonly CursorTracker _cursorTracker;
        private readonly FieldManipulatorSettings _settings;

        private readonly IFieldManipulatorOptions _options;

        /// <summary>
        /// Constructs a FieldManipulator instance that allows the cursor tracked by cursorTracker to interact with 
        /// fields defined on polyhedron.
        /// </summary>
        /// <param name="polyhedron"></param>
        /// <param name="cursorTracker"></param>
        /// <param name="options"></param>
        public FieldManipulator(IPolyhedron polyhedron, CursorTracker cursorTracker, IFieldManipulatorOptions options)
        {
            _polyhedron = polyhedron; 
            _cursorTracker = cursorTracker;
            _settings = new FieldManipulatorSettings(options);
            _options = options;

        }

        //TODO: If it modifies the field, it shouldn't return it. If it returns a field, it should be a copy of the field passed.
        /// <summary>
        /// Servant for Unity's Update(). Intended to be called once a frame. Takes a field and modifies it according
        /// to user interactions.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public ScalarField<Face> Update(ScalarField<Face> field)
        {
            _settings.Update();
            
            return AdjustedField(field);
        }

        // Takes a field and modifies it according to user interactions.
        private ScalarField<Face> AdjustedField(ScalarField<Face> field)
        {
            if (Input.GetKey(_options.RaiseSurfaceToolKey))
            {
                return TryAdjustFieldUnderCursor(field, Raise);
            }
            else if (Input.GetKey(_options.LowerSurfaceToolKey))
            {
                return TryAdjustFieldUnderCursor(field, Lower);
            }
            else
            {
                return field;
            }
        }

        // Applies the specified adjustmentFunction to the cells of the specified field under the cursor.
        private ScalarField<Face> TryAdjustFieldUnderCursor(ScalarField<Face> field, Func<double, double> adjustmentFunction)
        {
            var face = _cursorTracker.TryGetFaceUnderCursor();
            if (face != null)
            {
                var values = field.Values;
                var neighbours = GetNearbyFaces(face, _settings.AdjustmentRadius);
                foreach (var neighbour in neighbours)
                {
                    //TODO: This modifies the old values! Come up with a stateless alternative.
                    values[field.IndexOf(neighbour)] = adjustmentFunction(field[neighbour]);
                }

                return new ScalarField<Face>(field.IndexOf, values);
            }
            else
            {
                return field;
            }
        }

        // Gets the neighbours of a face which are within radius steps of it. 
        private IEnumerable<Face> GetNearbyFaces(Face center, int radius)
        {
            var faces = new HashSet<Face> {center};
            for (int i = 1; i < radius; i++)
            {
                var neighbours = faces.SelectMany(face => _polyhedron.NeighboursOf(face)).ToList();
                faces.UnionWith(neighbours);
            }

            return faces;
        }

        // Adjustment function which returns an incremented value.
        private double Raise(double x)
        {
            return x + _settings.AdjustmentSize;
        }

        // Adjustment function which returns a decremented value, clamped at zero.
        private double Lower(double x)
        {
            return Math.Max(x - _settings.AdjustmentSize, 0.0);
        }

        /// <summary>
        /// Servant for Unity's OnGUI function, intended to be called multiple times per frame.
        /// 
        /// Displays the current radius/intensity setting of the manipulator.
        /// </summary>
        public void OnGUI()
        {
            _settings.OnGUI();
        }
    }
}
