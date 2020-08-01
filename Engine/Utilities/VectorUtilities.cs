using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;

namespace Engine.Utilities
{
    using Vector = MathNet.Numerics.LinearAlgebra.Vector<double>;

    /// <summary>
    /// Static utility methods for Vector objects.
    /// </summary>
    public static class VectorUtilities
    {
        public static Vector CrossProduct(Vector a, Vector b) {
            return Vector.Build.Dense(new double[] {
                a[1]*b[2] - a[2]*b[1],
                a[2]*b[0] - a[0]*b[2],
                a[0]*b[1] - a[1]*b[0]});
        }

        public static double ScalarProduct(Vector a, Vector b) { 
            return a.DotProduct(b);
        }

        /// <summary>
        /// Calculates an approximation to the length of the spherical geodesic between a and b.
        /// 
        /// Is exact when a and b have the same norm.
        /// </summary>
        public static double GeodesicDistance(Vector a, Vector b)
        {
            var radius = (a.Norm(2) + b.Norm(2))/2;
            var angle = Trig.Acos(VectorUtilities.ScalarProduct(a.Normalize(2), b.Normalize(2)));

            return radius*angle;
        }

        /// <summary>
        /// Create a vector at the specified colatitude and azimuth at unit distance from the origin.
        /// </summary>
        public static Vector NewVector(double colatitude, double azimuth)
        {
            var x = Trig.Sin(colatitude) * Trig.Cos(azimuth);
            var y = Trig.Sin(colatitude) * Trig.Sin(azimuth);
            var z = Trig.Cos(colatitude);

            return Vector.Build.DenseOfArray(new [] {x, y, z});
        }

        /// <summary>
        /// Create a vector with the specified coordinates. 
        /// </summary>
        public static Vector NewVector(double x, double y, double z)
        {
            return Vector.Build.DenseOfArray(new [] {x, y, z});
        }

        /// <summary>
        /// Calculates the vector in the direction of destination that's perpendicular to origin.
        /// </summary>
        public static Vector LocalDirection(Vector origin, Vector destination)
        {
            var direction = destination - origin;
            var normalAtFrom = origin.Normalize(2);

            var localDirection = (direction - VectorUtilities.ScalarProduct(direction, normalAtFrom)*normalAtFrom).Normalize(2);

            return localDirection;
        }
    }
}
