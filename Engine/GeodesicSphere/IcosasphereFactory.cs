using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Geometry;

namespace Engine.GeodesicSphere
{
    public static class IcosasphereFactory
    {
        /// <summary>
        /// Constructs the icosasphere with least number of vertices exceeding the specified minimum. 
        /// </summary>
        public static IPolyhedron Build(IPolyhedronOptions options)
        {
            var icosahedron = IcosahedronFactory.Build();
            var numberOfSubdivisions = NumberOfSubdivisionsRequired(options.MinimumNumberOfFaces);

            for (int i = 0; i < numberOfSubdivisions; i++)
            {
                icosahedron = Subdivide(icosahedron);
            }

            return ProjectOntoSphere(icosahedron, options.Radius);
        }

        private static double NumberOfSubdivisionsRequired(int minimumNumberOfFaces)
        {
            var vertices = 12;
            var edges = 30;
            var faces = 20;

            var subdivisions = 0;
            while (faces < minimumNumberOfFaces)
            {
                vertices = vertices + edges;
                edges = 2*edges + 3*faces;
                faces = 4*faces;
                
                subdivisions = subdivisions + 1;
            }

            return subdivisions;
        }

        #region Subdivision methods.
        private static IPolyhedron Subdivide(IPolyhedron icosasphere)
        {
            var oldEdgesToNewVertices = CreateNewVerticesFrom(icosasphere.Edges);
            var newFaces = CreateFacesFrom(icosasphere.Faces, icosasphere.EdgesOf, oldEdgesToNewVertices).ToList();

            return new Polyhedron(newFaces);
        }

        private static List<List<Vertex>> CreateFacesFrom
            (List<Face> oldFaces, Func<Face, List<Edge>> oldFacesToOldEdges, Dictionary<Edge, Vertex> oldEdgesToNewVertices)
        {
            var newFaces = new List<List<Vertex>>();
            foreach (var oldFace in oldFaces)
            {
                newFaces.AddRange(CreateNewFacesFrom(oldFace, oldFacesToOldEdges, oldEdgesToNewVertices));
            }

            return newFaces;
        }

        private static List<List<Vertex>> CreateNewFacesFrom
            (Face oldFace, Func<Face, List<Edge>> oldFacesToOldEdges, Dictionary<Edge, Vertex> oldEdgesToNewVertices)
        {
            var newFaces = new List<List<Vertex>>();

            var edges = oldFacesToOldEdges(oldFace);
            foreach (var vertex in oldFace.Vertices)
            {
                var adjacentEdges = edges.Where(edge => edge.A == vertex || edge.B == vertex);
                var newVertices = adjacentEdges.Select(edge => oldEdgesToNewVertices[edge]).ToList();
                newVertices.Add(vertex);
                newFaces.Add(newVertices);
            }
            var centralFace = edges.Select(edge => oldEdgesToNewVertices[edge]).ToList();
            newFaces.Add(centralFace);

            return newFaces;
        }

        private static Dictionary<Edge, Vertex> CreateNewVerticesFrom(IEnumerable<Edge> edges)
        {
            return edges.Distinct().ToDictionary(edge => edge, edge => VertexAtMidpointOf(edge));
        }

        private static Vertex VertexAtMidpointOf(Edge edge)
        {
            var position = (edge.A.Position + edge.B.Position) / 2;

            return new Vertex(position);
        }
        #endregion

        private static IPolyhedron ProjectOntoSphere(IPolyhedron polyhedron, double radius)
        {
            var newVertex = 
                polyhedron.Vertices.
                ToDictionary(oldVertex => oldVertex, oldVertex => new Vertex(radius*oldVertex.Position.Normalize()));

            var newFaces =
                from face in polyhedron.Faces
                select face.Vertices.Select(oldVertex => newVertex[oldVertex]).ToList();

            return new Polyhedron(newFaces.ToList());
        }
    }
}
