using System.Collections.Generic;

namespace Engine.Geometry
{
    /// <summary>
    /// Represents a 3D polyhedron comprised of faces, edges and vertices.
    /// </summary>
    public interface IPolyhedron
    {
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
