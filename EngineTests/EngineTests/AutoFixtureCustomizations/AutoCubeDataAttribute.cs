using AutoFixture.Xunit2;

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
