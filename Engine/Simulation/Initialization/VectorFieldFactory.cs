using System;
using System.Linq;
using Engine.Geometry;
using Engine.Utilities;
using MathNet.Numerics.LinearAlgebra;

namespace Engine.Simulation.Initialization
{
    using Vector = MathNet.Numerics.LinearAlgebra.Vector<double>;
    public static class VectorFieldFactory
    {
        public static VectorField<Vertex> ConstantVectorField(IPolyhedron polyhedron, Vector average, double deviation)
        {
            var values = polyhedron.Vertices.Select(vertex => LocalVector(vertex.Position, average)).ToArray();

            return new VectorField<Vertex>(polyhedron.IndexOf, values);
        }

        public static VectorField<Vertex> RandomVectorField(IPolyhedron polyhedron, Vector average, double maxDeviation)
        {
            var prng = new Random();
            var values = polyhedron.Vertices.Select(vertex => RandomLocalVector(prng, vertex.Position, average, maxDeviation)).ToArray();

            return new VectorField<Vertex>(polyhedron.IndexOf, values);
        }

        private static Vector RandomLocalVector(Random prng, Vector origin, Vector average, double maxDeviation)
        {
            var x = maxDeviation*(prng.NextDouble() - 0.5);
            var y = maxDeviation*(prng.NextDouble() - 0.5);
            var vector = Vector.Build.DenseOfArray(new[] {x, y, 0});
            
            return LocalVector(origin, average + vector);
        }

        private static Vector LocalVector(Vector origin, Vector v)
        {
            var globalNorth = VectorUtilities.NewVector(0, 0, 1);

            var localEast = VectorUtilities.CrossProduct(globalNorth, origin).Normalize(2);
            var localNorth = VectorUtilities.CrossProduct(origin, localEast).Normalize(2);

            return v[0] * localNorth + v[1] * localEast;
        }
    }
}
