using System.Collections.Generic;
using System.Linq;
using Engine.Models;
using Engine.Polyhedra;
using UnityEngine;

namespace Assets.Views.ColorMap
{
    public class ColorMap
    {
        private readonly Mesh _mesh;
        private readonly IPolyhedron _polyhedron;

        private List<double> _maxes = new List<double>();
        private List<double> _mins = new List<double>();

        private readonly int[][] _faces;

        public ColorMap(Mesh mesh, IPolyhedron polyhedron)
        {
            _mesh = mesh;
            _polyhedron = polyhedron;

            _faces = VertexIndexedTableFactory.Faces(polyhedron);
        }

        public void Update(ScalarField<Face> field)
        {
            AddMaxAndMin(field.Values);

            var averageMax = _maxes.Average();
            var averageMin = _mins.Average();
            var gap = averageMax - averageMin <= 0 ? 1 : averageMax - averageMin; 

            //Debug.Log(String.Format("Min: {0,3:N2}, Max: {1,3:N2}", averageMin, averageMax));

            var colors = new Color[_polyhedron.Vertices.Count + _polyhedron.Faces.Count];
            var vertices = _polyhedron.Vertices;
            for (int index = 0; index < vertices.Count; index++)
            {
                var value = AverageAt(index, field);
                var color = ColorFromValue((float) ((value - averageMin)/gap));
                colors[index] = color;
            }

            var faces = _polyhedron.Faces;
            for (int index = 0; index < faces.Count; index++)
            {
                var value = field[index];
                var color = ColorFromValue((float) ((value - averageMin)/gap));
                colors[vertices.Count + index] = color;
            }

            _mesh.colors = colors;
        }

        private void AddMaxAndMin(double[] values)
        {
            _maxes.Add(values.Max());
            _mins.Add(values.Min());

            if (_maxes.Count > 1000)
            {
                _maxes = _maxes.Skip(1).ToList();
                _mins = _mins.Skip(1).ToList();
            }
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
