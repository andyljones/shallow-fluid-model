using Engine.Utilities;
using MathNet.Numerics.LinearAlgebra;

namespace Engine.Geometry
{
    public static class EdgeUtilities
    {
        public static double Length(this Edge edge)
        {
            return VectorUtilities.GeodesicDistance(edge.A.Position, edge.B.Position);
        }

        public static Vector SphericalCenter(this Edge edge)
        {
            return (edge.A.Position + edge.B.Position).Normalize();
        }
    }
}
