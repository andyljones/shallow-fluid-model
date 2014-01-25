using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;

namespace Engine.Utilities
{
    /// <summary>
    /// Comparer for ordering planar vectors in a clockwise fashion around a given point. 
    /// </summary>
    public class ClockwiseCompare : IComparer<Vector>
    {
        private readonly Vector _center;

        public ClockwiseCompare(Vector centerOfComparison)
        {
            _center = centerOfComparison;
        }

        public int Compare(Vector a, Vector b)
        {
            var middle = Vector.CrossProduct(a - _center, b - _center);
            var componentAlongCenter = Vector.ScalarProduct(middle, _center);

            return componentAlongCenter >= 0 ? 1 : -1;
        }
    }
}
