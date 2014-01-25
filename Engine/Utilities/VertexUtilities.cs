using Engine.Polyhedra;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;

namespace Engine.Utilities
{
    public static class VertexUtilities
    {
        /// <summary>
        /// Create a vertex at the specified colatitude and azimuth at unit distance from the origin.
        /// </summary>
        public static Vertex NewVertex(double colatitude, double azimuth)
        {
            var x = Trig.Sine(colatitude) * Trig.Cosine(azimuth);
            var y = Trig.Sine(colatitude) * Trig.Sine(azimuth);
            var z = Trig.Cosine(colatitude);

            var v = new Vector(new[] {x, y, z});

            return new Vertex(v);
        }
    }
}
