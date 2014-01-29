using System.Collections.Generic;
using System.Linq;
using Engine.Polyhedra;

namespace EngineTests.PolyhedraTests
{
    public static class CubeFactory
    {
        public static readonly Vertex[] UpperVertices =
        {
            VertexUtilities.NewVertex( 1, 1, 1),
            VertexUtilities.NewVertex( 1,-1, 1),
            VertexUtilities.NewVertex(-1,-1, 1),
            VertexUtilities.NewVertex(-1, 1, 1)
        };

        public static readonly Vertex[] LowerVertices =
        {
            VertexUtilities.NewVertex( 1, 1,-1),
            VertexUtilities.NewVertex( 1,-1,-1),
            VertexUtilities.NewVertex(-1,-1,-1),
            VertexUtilities.NewVertex(-1, 1,-1)
        };

        public static readonly IEnumerable<Vertex> NorthFace = UpperVertices;
        
        public static readonly IEnumerable<Vertex>[] MiddleFaces =
        {
            new[] {UpperVertices[0], UpperVertices[1], LowerVertices[0], LowerVertices[1]},
            new[] {UpperVertices[1], UpperVertices[2], LowerVertices[1], LowerVertices[2]},
            new[] {UpperVertices[2], UpperVertices[3], LowerVertices[2], LowerVertices[3]},
            new[] {UpperVertices[3], UpperVertices[0], LowerVertices[3], LowerVertices[0]}
        };

        public static readonly IEnumerable<Vertex> SouthFace = LowerVertices;

        public static IEnumerable<IEnumerable<Vertex>> VertexLists = MiddleFaces.Concat(new[] { NorthFace, SouthFace });

        public static IPolyhedron Build()
        {
            return new Polyhedron(VertexLists);
        }
    }
}
