using Engine.Simulation;
using EngineTests.PolyhedraTests;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit;

namespace EngineTests.AutoFixtureCustomizations
{
    public class AutoFieldIntegratorDataAttribute : AutoDataAttribute
    {
        public AutoFieldIntegratorDataAttribute(int iterations)
        {
            Fixture.Customize(new CubeCustomization());
            Fixture.Customize(new ScalarFieldCustomization());

            var simulationParameters = new SimulationParameters {NumberOfRelaxationIterations = iterations};
            Fixture.Inject(simulationParameters);
        }
    }
}
