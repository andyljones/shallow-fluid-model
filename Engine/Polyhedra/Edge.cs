using System.Collections.Generic;

namespace Engine.Polyhedra
{
    public class Edge
    {
        public readonly Vertex A;
        public readonly Vertex B;

        public Edge(Vertex a, Vertex b)
        {
            A = a;
            B = b;
        }

        public IEnumerable<Vertex> Vertices()
        {
            yield return A;
            yield return B;
        }
    }
}
