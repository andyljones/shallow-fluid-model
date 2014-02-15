using Engine.Utilities;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;

namespace Engine.Geometry
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
            return Trig.InverseCosine(vertex.Position.Normalize()[2]);
        }

        /// <summary>
        /// Azimuth of the vertex.
        /// </summary>
        public static double Azimuth(this Vertex vertex)
        {
            return Trig.InverseTangentFromRational(vertex.Position.Normalize()[1], vertex.Position.Normalize()[0]);            
        }

        /// <summary>
        /// Create a vertex at the specified colatitude and azimuth at unit distance from the origin.
        /// </summary>
        public static Vertex NewVertex(double colatitude, double azimuth)
        {
            return new Vertex(VectorUtilities.NewVector(colatitude, azimuth));
        }

        /// <summary>
        /// Create a vertex at the specified coordinates.
        /// </summary>
        public static Vertex NewVertex(double x, double y, double z)
        {
            var v = new Vector(new[] {x, y, z});

            return new Vertex(v);
        }
    }
}
