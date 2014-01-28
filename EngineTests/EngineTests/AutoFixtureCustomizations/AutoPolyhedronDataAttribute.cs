using Ploeh.AutoFixture.Xunit;

namespace EngineTests.AutoFixtureCustomizations
{
    public class AutoPolyhedronDataAttribute : AutoDataAttribute
    {
        public AutoPolyhedronDataAttribute()
        {
            Fixture.Customize(new CubeCustomization());
        }
    }
}
