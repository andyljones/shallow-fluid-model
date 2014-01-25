using Engine.Polyhedra;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;

namespace Engine.Utilities
{
    public static class VertexUtilities
    {
        /// <summary>
        /// X coordinate of the vertex.
        /// </summary>
        public static double X(this Vertex vertex)
        {
            return vertex.Position[0];
        }

        /// <summary>
        /// Y coordinate of the vertex.
        /// </summary>
        public static double Y(this Vertex vertex)
        {
            return vertex.Position[1];
        }

        /// <summary>
        /// Z coordinate of the vertex.
        /// </summary>
        public static double Z(this Vertex vertex)
        {
            return vertex.Position[2];
        }

        /// <summary>
        /// Colatitude of the vertex.
        /// </summary>
        public static double Colatitude(this Vertex vertex)
        {
            return Trig.InverseCosine(vertex.Z());
        }

        /// <summary>
        /// Azimuth of the vertex.
        /// </summary>
        public static double Azimuth(this Vertex vertex)
        {
            return Trig.InverseTangentFromRational(vertex.Y(), vertex.X());            
        }

        /// <summary>
        /// Create a vertex at the specified colatitude and azimuth at unit distance from the origin.
        /// </summary>
        public static Vertex NewVertex(double colatitude, double azimuth)
        {
            var x = Trig.Sine(colatitude) * Trig.Cosine(azimuth);
            var y = Trig.Sine(colatitude) * Trig.Sine(azimuth);
            var z = Trig.Cosine(colatitude);

            var v = new Vector(new[] { x, y, z });

            return new Vertex(v);
        }
    }
}
