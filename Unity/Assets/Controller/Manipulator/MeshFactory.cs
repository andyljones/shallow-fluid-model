using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Views;
using Engine.Geometry;
using Engine.Utilities;
using UnityEngine;

namespace Assets
{
    public static class MeshFactory
    {
        #region Build methods
        public static Mesh Build(IPolyhedron surface)
        {
            var vertices = CreateVertexArray(surface);
            var triangles = CreateTriangleArray(surface);
            
            return CreateMesh(vertices, triangles);
        }

        private static Vector3[] CreateVertexArray(IPolyhedron surface)
        {
            var vertexVectors = surface.Vertices.Select(vertex => GraphicsUtilities.Vector3(vertex.Position));
            var faceVectors = surface.Faces.Select(face => GraphicsUtilities.Vector3(face.SphericalCenter()));

            return vertexVectors.Concat(faceVectors).ToArray();
        }

        private static int[] CreateTriangleArray(IPolyhedron surface)
        {
            var triangles = surface.Faces.SelectMany(face => Triangles(surface, face)).ToArray();

            return triangles;
        }

        private static IEnumerable<int> Triangles(IPolyhedron surface, Face face)
        {
            var vertices = face.Vertices;
            var center = surface.Vertices.Count + surface.IndexOf(face);
            var triangles = new List<int>();
            for (int i = 0; i < vertices.Count; i++)
            {
                var triangle = new[]
                {
                    center,
                    surface.IndexOf(vertices.AtCyclicIndex(i + 1)),
                    surface.IndexOf(vertices.AtCyclicIndex(i))
                };
                triangles.AddRange(triangle);
            }

            return triangles;
        }

        private static Mesh CreateMesh(Vector3[] vertices, int[] triangles)
        {
            var mesh = new Mesh();
            mesh.vertices = vertices;

            mesh.subMeshCount = 1;
            mesh.SetIndices(triangles, MeshTopology.Triangles, 0);

            mesh.uv = Enumerable.Repeat(new Vector2(), mesh.vertexCount).ToArray();

            mesh.normals = mesh.vertices.Select(v => v.normalized).ToArray();

            return mesh;
        }
        #endregion

        #region TriangleIndexToFaceFunction methods
        public static Func<int, Face> TriangleIndexToFaceFunction(IPolyhedron surface)
        {
            var triangleIndexLookup = surface.Faces.SelectMany(face => Enumerable.Repeat(face, face.Vertices.Count)).ToArray();

            var closure = new TriangleIndexLookupClosure {FacesAtEachIndex = triangleIndexLookup};

            return closure.FaceAtTriangleIndex;
        }

        private class TriangleIndexLookupClosure
        {
            public Face[] FacesAtEachIndex;

            public Face FaceAtTriangleIndex(int i)
            {
                return FacesAtEachIndex[i];
            }
        }
        #endregion


        //private int[] CreateLineArray()
        //{
        //    return _polyhedron.Edges.SelectMany(edge => Line(edge)).ToArray();
        //}

        //private IEnumerable<int> Line(Edge edge)
        //{
        //    return new List<int> { _polyhedron.IndexOf(edge.B), _polyhedron.IndexOf(edge.A) };
        //}
    }
}
