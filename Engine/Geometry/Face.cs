using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Utilities;
using MathNet.Numerics.LinearAlgebra;

namespace Engine.Geometry
{
    using Vector = MathNet.Numerics.LinearAlgebra.Vector<double>;

    /// <summary>
    /// Represents a face in a IPolyhedron.
    /// 
    /// Two faces are equal if their collections of vertices are sequence equal.
    /// </summary>
    public class Face
    {
        /// <summary>
        /// The vertices comprising the face, sorted in anticlockwise order around the face's center 
        /// (when looking towards the origin)
        /// </summary>
        public readonly List<Vertex> Vertices;

        public Face(IEnumerable<Vertex> vertices)
        {
            Vertices = SortVertices(vertices);
        }

        // Sort the vertices in anticlockwise order when looking towards the origin.
        private static List<Vertex> SortVertices(IEnumerable<Vertex> vertices)
        {
            var vertexList = vertices.ToList();
            var center = vertexList.Aggregate(Vector.Build.Dense(3), (c, v) => c + v.Position).Normalize(2);
            var view = -center;
            var comparer = new AnticlockwiseComparer(vertexList.First().Position, view);

            var sortedVertices = vertexList.OrderBy(vertex => vertex.Position, comparer).ToList();

            return sortedVertices;
        }

        public override string ToString()
        {
            return String.Join(", ", Vertices.Select(vertex => vertex.ToString()).ToArray());
        }

        #region Equals, GetHashCode, (==) and (!=) overrides
        public static bool operator ==(Face a, Face b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            if ((object)a == null || (object)b == null)
            {
                return false;
            }

            return a.Equals(b);
        }

        public static bool operator !=(Face a, Face b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var face = obj as Face;
            if ((object)face == null)
            {
                return false;
            }

            return Equals(face);
        }

        public bool Equals(Face other)
        {
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (this.GetHashCode() != other.GetHashCode())
            {
                return false;
            }

            return this.Vertices.SequenceEqual(other.Vertices);
        }

        public override int GetHashCode()
        {
            return Vertices.Aggregate(0, (i, v) => i ^ v.GetHashCode());
        }
        #endregion
    }
}
