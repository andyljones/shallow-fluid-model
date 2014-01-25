using System.Collections.Generic;

namespace Engine.Polyhedra
{
    public static class FaceExtensions
    {
        /// <summary>
        /// Gets a list of the edges around a face.
        /// </summary>
        public static IEnumerable<Edge> Edges(this Face face)
        {
            var vertices = face.Vertices;

            var edges = new List<Edge>();
            for (int i = 0; i < vertices.Count-1; i++)
            {
                edges.Add(new Edge(vertices[i], vertices[i+1]));
            }
            edges.Add(new Edge(vertices[vertices.Count-1], vertices[0]));

            return edges;
        }
    }
}
