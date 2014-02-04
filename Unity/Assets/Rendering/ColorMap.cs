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

        private List<double> _maxes = new List<double> {double.MinValue};
        private List<double> _mins = new List<double> {double.MaxValue};

        private int[][] _faces;

        public ColorMap(GameObject surface, IPolyhedron polyhedron)
        {
            _meshFilter = surface.GetComponent<MeshFilter>();
            _polyhedron = polyhedron;

            _faces = VertexIndexedTableFactory.Faces(polyhedron);
        }

        public void Update(ScalarField<Face> field)
        {
            var max = _maxes.Average();
            var min = _mins.Average();
            var gap = max - min <= 0 ? 1 : max - min; 

            Debug.Log(String.Format("Min: {0,3:N2}, Max: {1,3:N2}", min, max));

        
            var colors = new Color[_polyhedron.Vertices.Count + _polyhedron.Faces.Count];
            var vertices = _polyhedron.Vertices;
            for (int index = 0; index < vertices.Count; index++)
            {
                var value = AverageAt(index, field);
                var color = ColorFromValue((float) ((value - min)/gap));
                colors[index] = color;

                max = max < value ? value : max;
                min = min > value ? value : min;
            }

            var faces = _polyhedron.Faces;
            for (int index = 0; index < faces.Count; index++)
            {
                var value = field[index];
                var color = ColorFromValue((float) ((value - min)/gap));
                colors[vertices.Count + index] = color;

                max = max < value ? value : max;
                min = min > value ? value : min;
            }

            AddMaxAndMin(max, min);


            _meshFilter.mesh.colors = colors;
        }

        private void AddMaxAndMin(double max, double min)
        {
            _maxes.Add(max);
            _mins.Add(min);

            if (_maxes.Count == 2 || _maxes.Count > 100)
            {
                _maxes = _maxes.Skip(1).ToList();
                _mins = _mins.Skip(1).ToList();
            }
        }

        //private double Average(IList<double> list)
        //{
        //    var sum = 0.0;
        //    for (int i = 0; i < list.Count; i++)
        //    {
        //        sum += list[i];
        //    }
        //    return sum/list.Count;
        //}

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
