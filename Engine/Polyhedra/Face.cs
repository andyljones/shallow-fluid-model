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
    }
}
