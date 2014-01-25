using System.Collections.Generic;
using System.Linq;
using Engine.Utilities;

namespace Engine.Polyhedra
{
    public static class FaceExtensions
    {
        /// <summary>
        /// Gets a list of the edges around a face.
        /// </summary>
        public static IEnumerable<Vertex[]> Edges(this Face face)
        {
            var vertices = face.Vertices;
            var edges = new List<Vertex[]>();
            for (int i = 0; i < vertices.Count-1; i++)
            {
                edges.Add(new [] {vertices[i], vertices[i+1]});
            }
            edges.Add(new [] {vertices[vertices.Count-1], vertices[0]});

            return edges;
        }
    }
}
