using System;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;

namespace Engine.Geometry
{
    using Vector = MathNet.Numerics.LinearAlgebra.Vector<double>;

    /// <summary>
    /// Represents a vertex in an IPolyhedron.
    /// </summary>
    public class Vertex
    {
        public readonly Vector Position;

        public Vertex(Vector position)
        {
            Position = position;
        }

        public override string ToString()
        {
            var colatitude = Trig.RadianToDegree(this.Colatitude());
            var azimuth = Trig.RadianToDegree(this.Azimuth());

            return String.Format("({0,3:N0}, {1,3:N0})", colatitude, azimuth);
        }

        #region GetHashCode override
        public override int GetHashCode()
        {
            return Position.GetHashCode();
        }
        #endregion
    }
}
