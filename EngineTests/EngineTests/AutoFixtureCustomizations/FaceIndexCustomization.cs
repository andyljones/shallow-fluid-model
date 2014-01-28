using System.Collections.Generic;
using System.Linq;
using Engine.Polyhedra;
using Ploeh.AutoFixture;

namespace EngineTests.PolyhedraTests
{
    public class FaceIndexCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            var polyhedron = fixture.Create<IPolyhedron>();
            var faces = polyhedron.Faces;
            var dictionary = Enumerable.Range(0, faces.Count).ToDictionary(i => faces[i]);

            fixture.Inject(dictionary);
        }
    }
}
