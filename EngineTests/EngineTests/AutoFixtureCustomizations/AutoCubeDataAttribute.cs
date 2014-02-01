using Ploeh.AutoFixture.Xunit;

namespace EngineTests.AutoFixtureCustomizations
{
    public class AutoCubeDataAttribute : AutoDataAttribute
    {
        public AutoCubeDataAttribute()
        {
            Fixture.Customize(new CubeCustomization());
        }
    }
}
