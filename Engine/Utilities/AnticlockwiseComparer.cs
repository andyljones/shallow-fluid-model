using System.Collections.Generic;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;

namespace Engine.Utilities
{
    /// <summary>
    /// Comparer for ordering planar vectors in an anticlockwise fashion around a given point. 
    /// </summary>
    public class AnticlockwiseComparer : IComparer<Vector>
    {
        private readonly Vector _center;
        private readonly Vector _viewVector;

        public AnticlockwiseComparer(Vector center, Vector view)
        {
            _center = center;
            _viewVector = view;
        }

        public int Compare(Vector a, Vector b)
        {
            if (Precision.AlmostEqual(a, b))
            {
                return 0;
            }

            if (Precision.AlmostEqual(a, _center))
            {
                return -1;
            }

            if (Precision.AlmostEqual(_center, b))
            {
                return 1;
            }

            var middle = Vector.CrossProduct(a - _center, b - _center);
            var componentAlongView = Vector.ScalarProduct(middle, _viewVector);

            var result = componentAlongView <= 0 ? -1 : 1;

            return result;
        }
    }
}
