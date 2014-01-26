using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;

namespace Engine.Polyhedra.IcosahedronBased
{
    public static class GeodesicSphereFactory
    {
        /// <summary>
        /// Constructs the geodesic sphere with least number of faces exceeding the specified minimum. 
        /// </summary>
        public static Polyhedron Build(int minimumNumberOfFaces)
        {
            var icosasphere = IcosasphereFactory.Build(minimumNumberOfFaces);
            var faces = DualofIcosasphere(icosasphere);

            return new Polyhedron(faces);
        }

        private static IEnumerable<IEnumerable<Vertex>> DualofIcosasphere(Polyhedron icosasphere)
        {
            var newVertexDict = icosasphere.Faces.ToDictionary(face => face, face => VertexAtCenterOf(face));
            var vertexLists = 
                icosasphere.Vertices.
                Select(oldVertex => CreateFaceAbout(oldVertex, newVertexDict, icosasphere.VertexToFaces));

            return vertexLists;
        }

        private static IEnumerable<Vertex> CreateFaceAbout(Vertex oldVertex, Dictionary<Face, Vertex> newVertexDict, Dictionary<Vertex, HashSet<Face>> oldFacesDict)
        {
            var oldFaces = oldFacesDict[oldVertex];
            var newVertices = oldFaces.Select(oldFace => newVertexDict[oldFace]);

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
