using Engine.Utilities;
using MathNet.Numerics.LinearAlgebra;

namespace Engine.Geometry
{
    /// <summary>
    /// Extension methods for the Edge class.
    /// </summary>
    public static class EdgeUtilities
    {
        /// <summary>
        /// Returns the length of the edge, measured along the geodesic connecting its vertices.
        /// </summary>
        /// <param name="edge"></param>
        /// <returns></returns>
        public static double Length(this Edge edge)
        {
            return VectorUtilities.GeodesicDistance(edge.A.Position, edge.B.Position);
        }

        /// <summary>
        /// Return the midpoint of the edge projected onto a unit sphere.
        /// </summary>
        /// <param name="edge"></param>
        /// <returns></returns>
        public static Vector SphericalCenter(this Edge edge)
        {
            return (edge.A.Position + edge.B.Position).Normalize();
        }
    }
}
