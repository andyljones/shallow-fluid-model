using MathNet.Numerics.LinearAlgebra;
using UnityEngine;

namespace Assets.Rendering
{
    public static class GraphicsUtilities
    {
        /// <summary>
        /// Converts a MathNet.Numerics vector to a Unity Vector3.
        /// </summary>
        public static Vector3 Vector3(Vector v)
        {
            var x =  (float) v[0];
            var y = -(float) v[1];
            var z =  (float) v[2];

            return new Vector3(x, y, z);
        }
    }
}
