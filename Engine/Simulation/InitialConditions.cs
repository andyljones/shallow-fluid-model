using System.Linq;
using Engine.Polyhedra;

namespace Engine.Simulation
{
    public class InitialConditions
    {
        public readonly PrognosticFields<Face> Fields;

        private readonly IPolyhedron _polyhedron;

        public InitialConditions(IPolyhedron polyhedron)
        {
            _polyhedron = polyhedron;
            Fields = GenerateInitialFields(polyhedron);
        }

        private PrognosticFields<Face> GenerateInitialFields(IPolyhedron surface)
        {
            var fields = new PrognosticFields<Face>
            {
                DerivativeOfAbsoluteVorticity = ConstantField(0),
                DerivativeOfDivergence = ConstantField(0),
                DerivativeOfHeight = ConstantField(0),
                AbsoluteVorticity = ConstantField(0),
                Divergence = ConstantField(0),
                Height = ConstantField(8),
                Streamfunction = ConstantField(0),
                VelocityPotential = ConstantField(0)
            };

            return fields;
        }

        private ScalarField<Face> ConstantField(double value)
        {
            var values = Enumerable.Repeat(value, _polyhedron.Faces.Count).ToArray();

            return new ScalarField<Face>(_polyhedron.IndexOf, values);
        }
    }
}
