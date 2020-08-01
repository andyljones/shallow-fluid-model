using EngineTests.GeometryTests;
using AutoFixture;

namespace EngineTests.AutoFixtureCustomizations
{
    public class CubeCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Inject(CubeFactory.VertexLists);
            fixture.Inject(CubeFactory.Build());
        }
    }
}
