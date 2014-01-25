using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Polyhedra;

namespace Engine.Icosasphere
{
    public static class IcosasphereSubdivider
    {
        public static Polyhedron Subdivide(Polyhedron polyhedron)
        {
            var oldEdgesToNewVertices = CreateNewVerticesFrom(polyhedron.Edges);
            var newFaces = CreateFacesFrom(polyhedron.Faces, polyhedron.FaceToEdgeDictionary, oldEdgesToNewVertices);

            return new Polyhedron(newFaces);
        }

        private static IEnumerable<IEnumerable<Vertex>> CreateFacesFrom(List<Face> faces, Dictionary<Face, HashSet<Edge>> faceToEdgeDictionary, Dictionary<Edge, Vertex> oldEdgesToNewVertices)
        {
            var newFaces = new List<IEnumerable<Vertex>>();
            foreach (var face in faces)
            {
                var edges = faceToEdgeDictionary[face];
                foreach (var vertex in face.Vertices)
                {
                    var adjacentEdges = edges.Where(edge => edge.A == vertex || edge.B == vertex);
                    var newVertices = adjacentEdges.Select(edge => oldEdgesToNewVertices[edge]).ToList();
                    newVertices.Add(vertex);
                    newFaces.Add(newVertices);
                }
                var centralFace = edges.Select(edge => oldEdgesToNewVertices[edge]).ToList();
                newFaces.Add(centralFace);
            }

            return newFaces;
        }

        private static Dictionary<Edge, Vertex> CreateNewVerticesFrom(List<Edge> edges)
        {
            return edges.Distinct().ToDictionary(edge => edge, edge => VertexAtMidpointOf(edge));
        }

        private static Vertex VertexAtMidpointOf(Edge edge)
        {
            var position = (edge.A.Position + edge.B.Position).Normalize();

            return new Vertex(position);
        }
    }
}
