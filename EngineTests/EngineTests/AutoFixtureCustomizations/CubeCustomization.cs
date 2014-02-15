using EngineTests.GeometryTests;
using Ploeh.AutoFixture;

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
