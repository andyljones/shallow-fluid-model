using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Polyhedra;
using MathNet.Numerics.LinearAlgebra;

namespace Engine.Icosasphere
{
    public class Geodesic : Polyhedron
    {
        public Geodesic(Polyhedron polyhedron) : base(CreateVertexLists(polyhedron)) { }

        private static IEnumerable<IEnumerable<Vertex>> CreateVertexLists(Polyhedron polyhedron)
        {
            var oldFaceToNewVertexDictionary = polyhedron.Faces.ToDictionary(face => face, face => VertexAtCenterOf(face));
            var vertexLists = polyhedron.Vertices.Select(oldVertex => CreateFaceAbout(oldVertex, oldFaceToNewVertexDictionary, polyhedron.VertexToFaceDictionary));

            return vertexLists;
        }

        private static IEnumerable<Vertex> CreateFaceAbout(Vertex oldVertex, Dictionary<Face, Vertex> oldFaceToNewVertexDictionary, Dictionary<Vertex, HashSet<Face>> vertexToFaceDictionary)
        {
            var oldFaces = vertexToFaceDictionary[oldVertex];
            var newVertices = oldFaces.Select(oldFace => oldFaceToNewVertexDictionary[oldFace]);

            return newVertices;
        }

        private static Vertex VertexAtCenterOf(Face face)
        {
            var a = face.Vertices[0].Position.Normalize();
            var b = face.Vertices[1].Position.Normalize();
            var c = face.Vertices[2].Position.Normalize();

            var center = Vector.CrossProduct(a - b, c - b).Normalize();

            return new Vertex(center);
        }
    }
}
