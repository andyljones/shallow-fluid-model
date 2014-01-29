using EngineTests.PolyhedraTests;
using Ploeh.AutoFixture.Xunit;

namespace EngineTests.AutoFixtureCustomizations
{
    public class AutoScalarFieldOnCubeDataAttribute : AutoDataAttribute
    {
        public AutoScalarFieldOnCubeDataAttribute()
        {
            Fixture.Customize(new CubeCustomization());
            Fixture.Customize(new FaceIndexCustomization());
            Fixture.Customize(new ScalarFieldCustomization());
        }
    }
}
