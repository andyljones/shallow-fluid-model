using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineTests.PolyhedraTests;
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
