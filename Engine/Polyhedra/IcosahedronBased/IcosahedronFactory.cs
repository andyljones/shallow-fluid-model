using System;
using System.Linq;
using Engine.Utilities;
using MathNet.Numerics;

namespace Engine.Polyhedra.IcosahedronBased
{
    /// <summary>
    /// A unit-radius icosahedron.
    /// </summary>
    public static class IcosahedronFactory
    {
        // The colatitudes of the northern five vertices and the southern five vertices.
        private static readonly double NorthernColatitude = Trig.InverseCosine((1 + Math.Sqrt(5))/(5 + Math.Sqrt(5)));
        private static readonly double SouthernColatitude = Math.PI - NorthernColatitude;

        // The 12 vertices of the icosahedron.
        private static readonly Vertex NorthPole = VertexUtilities.NewVertex(0, 0);
        private static readonly Vertex SouthPole = VertexUtilities.NewVertex(Math.PI, 0);

        private static readonly Vertex[] NorthernVertices = 
            Enumerable.Range(0, 5).Select(i => VertexUtilities.NewVertex(NorthernColatitude, i*2*Math.PI/5)).ToArray();
        private static readonly Vertex[] SouthernVertices = 
            Enumerable.Range(0, 5).Select(i => VertexUtilities.NewVertex(SouthernColatitude, i*2*Math.PI/5 + 2*Math.PI/10)).ToArray();

        // The five northmost faces of the icosahedron.
        private static readonly Vertex[][] NorthernFaces = 
        {
            new [] { NorthPole, NorthernVertices[0], NorthernVertices[1]},
            new [] { NorthPole, NorthernVertices[1], NorthernVertices[2]},
            new [] { NorthPole, NorthernVertices[2], NorthernVertices[3]},
            new [] { NorthPole, NorthernVertices[3], NorthernVertices[4]},
            new [] { NorthPole, NorthernVertices[4], NorthernVertices[0]}
        };

        // The five southmost faces of the icosahedron.
        private static readonly Vertex[][] SouthernFaces = 
        {
            new [] { SouthPole, SouthernVertices[0], SouthernVertices[1]},
            new [] { SouthPole, SouthernVertices[1], SouthernVertices[2]},
            new [] { SouthPole, SouthernVertices[2], SouthernVertices[3]},
            new [] { SouthPole, SouthernVertices[3], SouthernVertices[4]},
            new [] { SouthPole, SouthernVertices[4], SouthernVertices[0]}
        };

        // The five upper-middle faces of the icosahedron.
        private static readonly Vertex[][] UpperMiddleFaces =
        {
            new[] {NorthernVertices[0], SouthernVertices[0], NorthernVertices[1]},
            new[] {NorthernVertices[1], SouthernVertices[1], NorthernVertices[2]},
            new[] {NorthernVertices[2], SouthernVertices[2], NorthernVertices[3]},
            new[] {NorthernVertices[3], SouthernVertices[3], NorthernVertices[4]},
            new[] {NorthernVertices[4], SouthernVertices[4], NorthernVertices[0]}
        };

        // The five lower-middle faces of the icosahedron.
        private static readonly Vertex[][] LowerMiddleFaces =
        {
            new[] {SouthernVertices[0], NorthernVertices[1], SouthernVertices[1]},
            new[] {SouthernVertices[1], NorthernVertices[2], SouthernVertices[2]},
            new[] {SouthernVertices[2], NorthernVertices[3], SouthernVertices[3]},
            new[] {SouthernVertices[3], NorthernVertices[4], SouthernVertices[4]},
            new[] {SouthernVertices[4], NorthernVertices[0], SouthernVertices[0]}
        };

        // All the faces of the icosahedron.
        private static readonly Vertex[][] AllFaces =
            NorthernFaces
            .Concat(UpperMiddleFaces)
            .Concat(LowerMiddleFaces)
            .Concat(SouthernFaces)
            .ToArray();

        /// <summary>
        /// Constructs an icosahedron. 
        /// </summary>
        public static Polyhedron Build()
        {
            return new Polyhedron(AllFaces);
        }
    }
}
