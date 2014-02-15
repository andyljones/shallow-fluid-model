using System.Linq;
using Engine.Geometry;
using Engine.Simulation;
using UnityEngine;

namespace Assets.Views.ColorMap
{
    class FieldColorer
    {
        private readonly IPolyhedron _polyhedron;
        private readonly int[][] _faces;

        public FieldColorer(IPolyhedron polyhedron)
        {
            _polyhedron = polyhedron;

            _faces = VertexIndexedTableFactory.Faces(polyhedron);
        }

        public Color[] Color(ScalarField<Face> field)
        {
            var max = field.Values.Max();
            var min = field.Values.Min();
            var gap = max - min <= 0 ? 1 : max - min; 

            var colors = new Color[_polyhedron.Vertices.Count + _polyhedron.Faces.Count];
            var vertices = _polyhedron.Vertices;
            for (int index = 0; index < vertices.Count; index++)
            {
                var value = AverageAt(index, field);
                var color = ColorFromValue((float) ((value - min)/gap));
                colors[index] = color;
            }

            var faces = _polyhedron.Faces;
            for (int index = 0; index < faces.Count; index++)
            {
                var value = field[index];
                var color = ColorFromValue((float) ((value - min)/gap));
                colors[vertices.Count + index] = color;
            }

            return colors;
        }

        private double AverageAt(int vertex, ScalarField<Face> field)
        {
            var faces = _faces[vertex];
            return faces.Average(i => field[i]);
        }

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
