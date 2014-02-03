using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Models;
using Engine.Polyhedra;
using UnityEngine;

namespace Assets.Rendering
{
    public class ColorMap
    {
        private readonly MeshFilter _meshFilter;
        private readonly IPolyhedron _polyhedron;

        private List<double> _maxes = new List<double>();
        private List<double> _mins = new List<double>();

        public ColorMap(GameObject surface, IPolyhedron polyhedron)
        {
            _meshFilter = surface.GetComponent<MeshFilter>();
            _polyhedron = polyhedron;
        }

        public void Update(ScalarField<Face> field)
        {
            var valuesAtVertices = _polyhedron.Vertices.Select(vertex => AverageAt(vertex, field));
            var valuesAtFaces = field.Values;
            var values = valuesAtVertices.Concat(valuesAtFaces).ToArray();

            AddMaxAndMin(values);

            var max = _maxes.Average();
            var min = _mins.Average();
            var gap = max - min <= 0 ? 1 : max - min; 

            Debug.Log(String.Format("Min: {0,3:N2}, Max: {1,3:N2}", min, max));

            var normalizedValues = values.Select(value => (value - min)/gap).ToArray();
            var colors = normalizedValues.Select(average => ColorFromValue((float)average)).ToArray();

            _meshFilter.mesh.colors = colors;
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

        private double AverageAt(Vertex vertex, ScalarField<Face> field)
        {
            return _polyhedron.FacesOf(vertex).Average(face => field[face]);
        }

        private double AverageAt(Edge edge, ScalarField<Face> field)
        {
            return _polyhedron.FacesOf(edge).Average(face => field[face]);
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
