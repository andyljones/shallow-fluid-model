using System;
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
        public static IPolyhedron Build(IPolyhedronOptions options)
        {
            var minimumNumberOfVertices = 2*options.MinimumNumberOfFaces - 4;
            var icosasphereOptions = new Options
            {
                Radius = options.Radius,
                MinimumNumberOfFaces = minimumNumberOfVertices
            };

            var icosasphere = IcosasphereFactory.Build(icosasphereOptions);
            var faces = DualofIcosasphere(icosasphere);

            return new Polyhedron(faces);
        }

        private static IEnumerable<IEnumerable<Vertex>> DualofIcosasphere(IPolyhedron icosasphere)
        {
            var newVertexDict = icosasphere.Faces.ToDictionary(face => face, face => VertexAtCenterOf(face));
            var vertexLists = 
                icosasphere.Vertices.
                Select(oldVertex => CreateFaceAbout(oldVertex, newVertexDict, icosasphere.FacesOf));

            return vertexLists;
        }

        private static IEnumerable<Vertex> CreateFaceAbout(Vertex oldVertex, Dictionary<Face, Vertex> newVertexDict, Func<Vertex, List<Face>> oldFacesDict)
        {
            var oldFaces = oldFacesDict(oldVertex);
            var newVertices = oldFaces.Select(oldFace => newVertexDict[oldFace]);

            return newVertices;
        }

        private static Vertex VertexAtCenterOf(Face face)
        {
            var a = face.Vertices[0].Position;
            var b = face.Vertices[1].Position;
            var c = face.Vertices[2].Position;

            var radius = (a.Norm() + b.Norm() + c.Norm())/3;

            var center = radius*Vector.CrossProduct(a - b, c - b).Normalize();

            return new Vertex(center);
        }
    }
}
