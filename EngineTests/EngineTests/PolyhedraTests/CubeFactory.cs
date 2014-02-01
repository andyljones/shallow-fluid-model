using System.Collections.Generic;
using System.Linq;
using Engine.Polyhedra;

namespace EngineTests.PolyhedraTests
{
    public static class CubeFactory
    {
        public static readonly List<Vertex> UpperVertices = new List<Vertex>
        {
            VertexUtilities.NewVertex( 1, 1, 1),
            VertexUtilities.NewVertex( 1,-1, 1),
            VertexUtilities.NewVertex(-1,-1, 1),
            VertexUtilities.NewVertex(-1, 1, 1)
        };

        public static readonly List<Vertex> LowerVertices = new List<Vertex>
        {
            VertexUtilities.NewVertex( 1, 1,-1),
            VertexUtilities.NewVertex( 1,-1,-1),
            VertexUtilities.NewVertex(-1,-1,-1),
            VertexUtilities.NewVertex(-1, 1,-1)
        };

        public static readonly List<Vertex> NorthFace = UpperVertices;
        
        public static readonly List<List<Vertex>> MiddleFaces = new List<List<Vertex>>
        {
            new List<Vertex> {UpperVertices[0], UpperVertices[1], LowerVertices[0], LowerVertices[1]},
            new List<Vertex> {UpperVertices[1], UpperVertices[2], LowerVertices[1], LowerVertices[2]},
            new List<Vertex> {UpperVertices[2], UpperVertices[3], LowerVertices[2], LowerVertices[3]},
            new List<Vertex> {UpperVertices[3], UpperVertices[0], LowerVertices[3], LowerVertices[0]}
        };

        public static readonly List<Vertex> SouthFace = LowerVertices;

        public static List<List<Vertex>> VertexLists = MiddleFaces.Concat(new[] { NorthFace, SouthFace }).ToList();

        public static IPolyhedron Build()
        {
            return new Polyhedron(VertexLists);
        }
    }
}
