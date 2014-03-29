using System.Linq;
using Engine.Geometry;
using Engine.Simulation;
using UnityEngine;

namespace Assets.Views.Level.ColorMap
{
    /// <summary>
    /// Calculates the color to display for each face on a simulation surface. Maintains a running average of 
    /// field maxima and minima so that changes in the range of values can be adapted to.
    /// </summary>
    class FieldColorer
    {
        private readonly IPolyhedron _polyhedron;
        private readonly int[][] _faces;
        private readonly MaxAndMinTracker _maxAndMinTracker;

        /// <summary>
        /// Construct a color mapper for the given polyhedron and maintain the running averages of the extrema for 
        /// the specified length of time.
        /// </summary>
        /// <param name="polyhedron"></param>
        /// <param name="lengthOfHistory"></param>
        public FieldColorer(IPolyhedron polyhedron, int lengthOfHistory)
        {
            _polyhedron = polyhedron;

            _faces = VertexIndexedTableFactory.Faces(polyhedron);
            _maxAndMinTracker = new MaxAndMinTracker(lengthOfHistory);
        }

        //TODO: Return a map from faces to colors rather than an array?
        /// <summary>
        /// Calculates the colors that should be displayed for each face. The resultant color array is indexed in the 
        /// same order as the field.
        /// 
        /// Blue corresponds to points at or below the running minima, red to points at or above the running maxima.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public Color[] Color(ScalarField<Face> field)
        {
            // Update the running averages of the extrema
            _maxAndMinTracker.Update(field.Values);

            // Get the current extrema
            var max = _maxAndMinTracker.AverageMax;
            var min = _maxAndMinTracker.AverageMin;
            var gap = max - min <= 0 ? 1 : max - min; 

            var colors = new Color[_polyhedron.Vertices.Count + _polyhedron.Faces.Count];

            // Color each vertex by averaging the field values of the faces around it.
            var vertices = _polyhedron.Vertices;
            for (int index = 0; index < vertices.Count; index++)
            {
                var value = AverageAt(index, field);
                // Choose the color of the vertex by normalizing it's value with respect to the running averages of the extrema.
                var color = ColorFromValue((float) ((value - min)/gap));
                colors[index] = color;
            }

            // Color each face 
            var faces = _polyhedron.Faces;
            for (int index = 0; index < faces.Count; index++)
            {
                var value = field[index];
                // Choose the color of the vertex by normalizing it's value with respect to the running averages of the extrema.
                var color = ColorFromValue((float) ((value - min)/gap));
                colors[vertices.Count + index] = color;
            }

            return colors;
        }

        // Calculates the average value of a field at a vertex by averaging over the value of the field at the faces around it.
        private double AverageAt(int vertex, ScalarField<Face> field)
        {
            var faces = _faces[vertex];
            return faces.Average(i => field[i]);
        }

        // Maps a number to a color. Zero maps to blue, one to red, and there's a smooth transition through green in between.
        private Color ColorFromValue(float x)
        {
            x = x >= 1 ? 1 : x <= 0 ? 0 : x;

            var r = -Mathf.Cos(1.5f * Mathf.PI * x) / 2 + 0.5f;
            var g = Mathf.Sin(1.5f * Mathf.PI * x);
            var b = Mathf.Cos(1.5f * Mathf.PI * x) / 2 + 0.5f;

            return new Color(r, g, b);
        }
    }
}
