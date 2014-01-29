using MathNet.Numerics.LinearAlgebra;

namespace Assets.Rendering
{
    public struct TangentSpace
    {
        public readonly Vector East;
        public readonly Vector North;
        public readonly Vector Up;

        public TangentSpace(Vector east, Vector north, Vector up)
        {
            East = east.Normalize();
            North = north.Normalize();
            Up = up.Normalize();
        }

        public Vector ToEuclideanSpace(Vector vector)
        {
            return vector[0]*East + vector[1]*North + vector[2]*Up;
        }
    }
}
