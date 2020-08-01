using AutoFixture.Xunit2;

namespace EngineTests.AutoFixtureCustomizations
{
    public class AutoFieldsOnCubeDataAttribute : AutoDataAttribute
    {
        public AutoFieldsOnCubeDataAttribute()
        {
            Fixture.Customize(new CubeCustomization());
            Fixture.Customize(new ScalarFieldCustomization());
            Fixture.Customize(new VectorFieldCustomization());
        }
    }
}
