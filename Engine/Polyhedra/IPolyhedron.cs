using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Polyhedra
{
    public interface IPolyhedron
    {
        /// <summary>
        /// The faces of the polyhedron, with their vertices listed in clockwise order when looking toward the origin.
        /// </summary>
        List<Face> Faces { get; }
        List<Edge> Edges { get; }
        List<Vertex> Vertices { get; }
        HashSet<Edge> EdgesOf(Vertex vertex);
        HashSet<Edge> EdgesOf(Face face);
        HashSet<Face> FacesOf(Vertex vertex);
        HashSet<Face> FacesOf(Edge edge);
    }
}
