using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;

namespace Engine.Utilities
{
    public static class VectorUtilities
    {
        /// <summary>
        /// Calculates an approximation to the length of the spherical geodesic between a and b.
        /// 
        /// Is exact when a and b have the same norm.
        /// </summary>
        public static double GeodesicDistance(Vector a, Vector b)
        {
            var radius = (a.Norm() + b.Norm())/2;
            var angle = Trig.InverseCosine(Vector.ScalarProduct(a.Normalize(), b.Normalize()));

            return radius*angle;
        }

        /// <summary>
        /// Create a vector at the specified colatitude and azimuth at unit distance from the origin.
        /// </summary>
        public static Vector NewVector(double colatitude, double azimuth)
        {
            var x = Trig.Sine(colatitude) * Trig.Cosine(azimuth);
            var y = Trig.Sine(colatitude) * Trig.Sine(azimuth);
            var z = Trig.Cosine(colatitude);

            return new Vector(new [] {x, y, z});
        }

        public static Vector LocalDirection(Vector from, Vector to)
        {
            var direction = to - from;
            var normalAtFrom = from.Normalize();

            var localDirection = (direction - Vector.ScalarProduct(direction, normalAtFrom)*normalAtFrom).Normalize();

            return localDirection;
        }
    }
}
