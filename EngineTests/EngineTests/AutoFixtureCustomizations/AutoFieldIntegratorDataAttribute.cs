using Engine.Simulation;
using EngineTests.SimulationTests;
using AutoFixture;
using AutoFixture.Xunit2;

namespace EngineTests.AutoFixtureCustomizations
{
    public class AutoFieldIntegratorDataAttribute : AutoDataAttribute
    {
        public AutoFieldIntegratorDataAttribute(int iterations)
        {
            Fixture.Customize(new CubeCustomization());
            Fixture.Customize(new ScalarFieldCustomization());

            var simulationParameters = new TestSimulationOptions {NumberOfRelaxationIterations = iterations};
            Fixture.Inject(simulationParameters);
        }
    }
}
