using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Controllers.Cursor;
using Engine.Geometry;
using Engine.Simulation;
using UnityEngine;

namespace Assets.Controllers.Manipulator
{
    public class FieldManipulator
    {
        public KeyCode SurfaceRaiseButton = KeyCode.UpArrow;
        public KeyCode SurfaceLowerButton = KeyCode.DownArrow;

        private readonly CursorTracker _cursorTracker;
        private readonly FieldManipulatorProperties _properties;
        private readonly IPolyhedron _polyhedron;

        public FieldManipulator(IPolyhedron polyhedron, CursorTracker cursorTracker)
        {
            _polyhedron = polyhedron; 
            _cursorTracker = cursorTracker;
            _properties = new FieldManipulatorProperties();
        }

        public ScalarField<Face> Update(ScalarField<Face> field)
        {
            _properties.Update();
            
            return AdjustedField(field);
        }

        private ScalarField<Face> AdjustedField(ScalarField<Face> field)
        {
            if (Input.GetKey(SurfaceRaiseButton))
            {
                return TryAdjustFieldUnderCursor(field, Raise);
            }
            else if (Input.GetKey(SurfaceLowerButton))
            {
                return TryAdjustFieldUnderCursor(field, Lower);
            }
            else
            {
                return field;
            }
        }

        private ScalarField<Face> TryAdjustFieldUnderCursor(ScalarField<Face> field, Func<double, double> adjustmentFunction)
        {
            var face = _cursorTracker.TryGetFaceUnderCursor();
            if (face != null)
            {
                var values = field.Values;
                var neighbours = GetNearbyFaces(face, _properties.Radius);
                foreach (var neighbour in neighbours)
                {
                    //TODO: This modifies the old values!
                    values[field.IndexOf(neighbour)] = adjustmentFunction(field[neighbour]);
                }

                return new ScalarField<Face>(field.IndexOf, values);
            }
            else
            {
                return field;
            }
        }

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

        private double Raise(double x)
        {
            return x + _properties.AdjustmentSize;
        }

        private double Lower(double x)
        {
            return x - _properties.AdjustmentSize;
        }


        public void UpdateGUI()
        {
            _properties.UpdateGUI();
        }
    }
}
