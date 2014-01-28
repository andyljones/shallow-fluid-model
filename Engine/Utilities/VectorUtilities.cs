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
    }
}
