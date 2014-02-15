using System;
using System.Linq;
using Engine.Geometry;
using Engine.Utilities;
using MathNet.Numerics.LinearAlgebra;

namespace Engine.Simulation.Initialization
{
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
            var values = polyhedron.Vertices.Select(vertex => average + RandomLocalVector(prng, vertex.Position, maxDeviation)).ToArray();

            return new VectorField<Vertex>(polyhedron.IndexOf, values);
        }

        private static Vector RandomLocalVector(Random prng, Vector origin, double maxMagnitude)
        {
            var x = maxMagnitude*(prng.NextDouble() - 0.5);
            var y = maxMagnitude*(prng.NextDouble() - 0.5);
            var v = new Vector(new[] {x, y});
            
            return LocalVector(origin, v);
        }

        private static Vector LocalVector(Vector origin, Vector v)
        {
            var globalNorth = VectorUtilities.NewVector(0, 0, 1);

            var localEast = Vector.CrossProduct(globalNorth, origin).Normalize();
            var localNorth = Vector.CrossProduct(origin, localEast).Normalize();

            return v[0] * localNorth + v[1] * localEast;
        }
    }
}
