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

        List<Edge> EdgesOf(Vertex vertex);
        List<Edge> EdgesOf(Face face);
        List<Face> FacesOf(Vertex vertex);
        List<Face> FacesOf(Edge edge);

        int IndexOf(Face face);
        int IndexOf(Edge edge);
        int IndexOf(Vertex vertex);
    }
}
