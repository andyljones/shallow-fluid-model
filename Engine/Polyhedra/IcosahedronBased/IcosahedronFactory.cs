using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Utilities;
using MathNet.Numerics;

namespace Engine.Polyhedra.IcosahedronBased
{
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
        private static readonly List<List<Vertex>> NorthernFaces = new List<List<Vertex>>
        {
            new List<Vertex> { NorthPole, NorthernVertices[0], NorthernVertices[1]},
            new List<Vertex> { NorthPole, NorthernVertices[1], NorthernVertices[2]},
            new List<Vertex> { NorthPole, NorthernVertices[2], NorthernVertices[3]},
            new List<Vertex> { NorthPole, NorthernVertices[3], NorthernVertices[4]},
            new List<Vertex> { NorthPole, NorthernVertices[4], NorthernVertices[0]}
        };

        // The five southmost faces of the icosahedron.
        private static readonly List<List<Vertex>> SouthernFaces = new List<List<Vertex>>
        {
            new List<Vertex> { SouthPole, SouthernVertices[0], SouthernVertices[1]},
            new List<Vertex> { SouthPole, SouthernVertices[1], SouthernVertices[2]},
            new List<Vertex> { SouthPole, SouthernVertices[2], SouthernVertices[3]},
            new List<Vertex> { SouthPole, SouthernVertices[3], SouthernVertices[4]},
            new List<Vertex> { SouthPole, SouthernVertices[4], SouthernVertices[0]}
        };

        // The five upper-middle faces of the icosahedron.
        private static readonly List<List<Vertex>> UpperMiddleFaces = new List<List<Vertex>>
        {
            new List<Vertex> {NorthernVertices[0], SouthernVertices[0], NorthernVertices[1]},
            new List<Vertex> {NorthernVertices[1], SouthernVertices[1], NorthernVertices[2]},
            new List<Vertex> {NorthernVertices[2], SouthernVertices[2], NorthernVertices[3]},
            new List<Vertex> {NorthernVertices[3], SouthernVertices[3], NorthernVertices[4]},
            new List<Vertex> {NorthernVertices[4], SouthernVertices[4], NorthernVertices[0]}
        };

        // The five lower-middle faces of the icosahedron.
        private static readonly List<List<Vertex>> LowerMiddleFaces = new List<List<Vertex>>
        {
            new List<Vertex> {SouthernVertices[0], NorthernVertices[1], SouthernVertices[1]},
            new List<Vertex> {SouthernVertices[1], NorthernVertices[2], SouthernVertices[2]},
            new List<Vertex> {SouthernVertices[2], NorthernVertices[3], SouthernVertices[3]},
            new List<Vertex> {SouthernVertices[3], NorthernVertices[4], SouthernVertices[4]},
            new List<Vertex> {SouthernVertices[4], NorthernVertices[0], SouthernVertices[0]}
        };

        /// <summary>
        /// Constructs an icosahedron. 
        /// </summary>
        public static IPolyhedron Build()
        {
            var allFaces = NorthernFaces
            .Concat(UpperMiddleFaces)
            .Concat(LowerMiddleFaces)
            .Concat(SouthernFaces)
            .ToList();

            return new Polyhedron(allFaces);
        }
    }
}
