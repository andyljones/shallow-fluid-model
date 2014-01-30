using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Polyhedra;
using Engine.Simulation;
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

        public PolyhedronRenderer(IPolyhedron polyhedron, String name, String wireframeMaterialName = "", String surfaceMaterialName = "")
        {
            _gameObject = GraphicsUtilities.CreateRenderingObject(name, CreateVertexArray(polyhedron.Vertices));

            if (wireframeMaterialName != "")
            { 
                GraphicsUtilities.AddWireframe(_gameObject, CreateLineArray(polyhedron.IndexOf, polyhedron.Edges), wireframeMaterialName);
            }

            if (surfaceMaterialName != "")
            {
                GraphicsUtilities.AddSurface(_gameObject, CreateTriangleArray(polyhedron.IndexOf, polyhedron.Faces), surfaceMaterialName);
            }

            _colorMap = new ColorMap(_gameObject, polyhedron);
        }

        #region CreateMesh methods
        private static Vector3[] CreateVertexArray(IEnumerable<Vertex> vertices)
        {
            return vertices.Select(vertex => GraphicsUtilities.Vector3(vertex.Position)).ToArray();
        }

        private static int[] CreateTriangleArray(Func<Vertex, int> indexOf, IEnumerable<Face> faces)
        {
            return faces.SelectMany(face => Indices(indexOf, face)).ToArray();
        }

        private static int[] CreateLineArray(Func<Vertex, int> indexOf, IEnumerable<Edge> edges)
        {
            return edges.SelectMany(edge => Indices(indexOf, edge)).ToArray();
        }

        private static IEnumerable<int> Indices(Func<Vertex, int> indexOf, Face face)
        {
            var vertices = face.Vertices;
            var triangles = new List<int>();
            for (int i = 1; i < vertices.Count-1; i++)
            {
                var triangle = new [] { indexOf(vertices[0]), indexOf(vertices[i]), indexOf(vertices[i+1]) };
                triangles.AddRange(triangle);
            }
            return triangles;
        }

        private static IEnumerable<int> Indices(Func<Vertex, int> indexOf, Edge edge)
        {
            return new List<int> {indexOf(edge.A), indexOf(edge.B)};
        }
        #endregion

        public void Update(ScalarField<Face> field)
        {
            _colorMap.Update(field);
        }
    }
}
