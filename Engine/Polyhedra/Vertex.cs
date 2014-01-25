using MathNet.Numerics.LinearAlgebra;

namespace Engine.Polyhedra
{
    public class Vertex
    {
        public Vector Position { get; set; }

        public Vertex(Vector position)
        {
            Position = position;
        }
    }
}
