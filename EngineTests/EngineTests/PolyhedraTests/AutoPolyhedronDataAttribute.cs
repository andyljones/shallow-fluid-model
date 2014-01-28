using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit;

namespace EngineTests.PolyhedraTests
{
    public class AutoPolyhedronDataAttribute : AutoDataAttribute
    {
        public AutoPolyhedronDataAttribute()
        {
            Fixture.Inject(CubeFactory.VertexLists);
            Fixture.Inject(CubeFactory.Build());
        }
    }
}
