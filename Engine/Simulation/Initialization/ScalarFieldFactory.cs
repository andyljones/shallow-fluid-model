using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Geometry;

namespace Engine.Simulation.Initialization
{
    public static class ScalarFieldFactory
    {
        public static ScalarField<Face> ConstantScalarField(IPolyhedron polyhedron, double average, double deviation)
        {
            var values = Enumerable.Repeat(average, polyhedron.Faces.Count).ToArray();

            return new ScalarField<Face>(polyhedron.IndexOf, values);
        }

        public static ScalarField<Face> RandomScalarField(IPolyhedron polyhedron, double average, double deviation)
        {
            var prng = new Random();
            var values = Enumerable.Repeat(deviation, polyhedron.Faces.Count).Select(i => average + (prng.NextDouble() - 0.5) * deviation).ToArray();

            return new ScalarField<Face>(polyhedron.IndexOf, values);
        }

        public static ScalarField<Face> XDependentScalarField(IPolyhedron polyhedron, double average, double deviation)
        {
            var normals = FaceIndexedTableFactory.Normals(polyhedron);
            var values = polyhedron.Faces.Select(face => average + deviation * normals[polyhedron.IndexOf(face)][0]).ToArray();

            return new ScalarField<Face>(polyhedron.IndexOf, values);
        }
    }
}
