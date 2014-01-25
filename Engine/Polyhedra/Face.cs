using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Engine.Polyhedra
{
    public class Face
    {
        public ReadOnlyCollection<Vertex> Vertices { get; private set; }

        public Face(IEnumerable<Vertex> vertices)
        {
            Vertices = new ReadOnlyCollection<Vertex>(vertices.ToList());
        }

        public override string ToString()
        {
            return String.Join(", ", Vertices.Select(vertex => vertex.ToString()).ToArray());
        }
    }
}
