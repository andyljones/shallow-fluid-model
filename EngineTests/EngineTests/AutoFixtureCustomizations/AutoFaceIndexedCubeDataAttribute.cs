using EngineTests.PolyhedraTests;
using Ploeh.AutoFixture.Xunit;

namespace EngineTests.AutoFixtureCustomizations
{
    public class AutoFaceIndexedCubeDataAttribute : AutoDataAttribute
    {
        public AutoFaceIndexedCubeDataAttribute()
        {
            Fixture.Customize(new CubeCustomization());
            Fixture.Customize(new FaceIndexCustomization());
        }
    }
}
