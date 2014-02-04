using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Models;
using Engine.Polyhedra;
using UnityEngine;

namespace Assets.Rendering
{
    /// <summary>
    /// Renders a Polyhedron as a solid object with the requested properties.
    /// </summary>
    public class PolyhedronRenderer
    {
        private readonly GameObject _gameObject;
        private readonly ColorMap _colorMap;

        private readonly IPolyhedron _polyhedron;

        public PolyhedronRenderer(IPolyhedron polyhedron, String name, String wireframeMaterialName = "", String surfaceMaterialName = "")
        {
            _polyhedron = polyhedron;            
            _gameObject = GraphicsUtilities.CreateRenderingObject(name, CreateVectorArray());

            if (wireframeMaterialName != "")
            { 
                GraphicsUtilities.AddWireframe(_gameObject, CreateLineArray(), wireframeMaterialName);
            }

            if (surfaceMaterialName != "")
            {
                GraphicsUtilities.AddSurface(_gameObject, CreateTriangleArray(), surfaceMaterialName);
            }

            _colorMap = new ColorMap(_gameObject, polyhedron);
        }

        #region CreateMesh methods
        private Vector3[] CreateVectorArray()
        {
            var vertexVectors = _polyhedron.Vertices.Select(vertex => GraphicsUtilities.Vector3(vertex.Position));
            var faceVectors = _polyhedron.Faces.Select(face => GraphicsUtilities.Vector3(face.SphericalCenter()));

            return vertexVectors.Concat(faceVectors).ToArray();
        }

        private int[] CreateTriangleArray()
        {
            return _polyhedron.Faces.SelectMany(face => Triangles(face)).ToArray();
        }

        private int[] CreateLineArray()
        {
            return _polyhedron.Edges.SelectMany(edge => Line(edge)).ToArray();
        }

        private IEnumerable<int> Triangles(Face face)
        {
            var vertices = face.Vertices;
            var center = _polyhedron.Vertices.Count + _polyhedron.IndexOf(face);
            var triangles = new List<int>();
            for (int i = 0; i < vertices.Count-1; i++)
            {
                var triangle = new[]
                {
                    center,
                    _polyhedron.IndexOf(vertices[i]),
                    _polyhedron.IndexOf(vertices[i + 1])
                };
                triangles.AddRange(triangle);
            }
            var lastTriangle = new[]
            {
                center,
                _polyhedron.IndexOf(vertices[vertices.Count - 1]),
                _polyhedron.IndexOf(vertices[0]),
            };
            triangles.AddRange(lastTriangle);

            triangles.Reverse();

            return triangles;
        }

        private IEnumerable<int> Line(Edge edge)
        {
            return new List<int> { _polyhedron.IndexOf(edge.B), _polyhedron.IndexOf(edge.A) };
        }
        #endregion

        public void Update(ScalarField<Face> field)
        {
            _colorMap.Update(field);
        }
    }
}
