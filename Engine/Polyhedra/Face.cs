using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Polyhedra
{
    public class Face
    {
        public IEnumerable<Vertex> Vertices { get; private set; }

        public Face(IEnumerable<Vertex> vertices)
        {
            Vertices = vertices.ToList();
        }

        public override string ToString()
        {
            return String.Join(", ", Vertices.Select(vertex => vertex.ToString()).ToArray());
        }
    }
}
