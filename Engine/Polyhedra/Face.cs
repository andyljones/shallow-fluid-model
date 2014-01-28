using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Engine.Utilities;
using MathNet.Numerics.LinearAlgebra;

namespace Engine.Polyhedra
{
    public class Face
    {
        public readonly List<Vertex> Vertices;

        public Face(IEnumerable<Vertex> vertices)
        {
            Vertices = SortVerticesClockwise(vertices).ToList();
        }

        private static IEnumerable<Vertex> SortVerticesClockwise(IEnumerable<Vertex> vertices)
        {
            var vertexList = vertices.ToList();
            var centroid = vertexList.Aggregate(Vector.Zeros(3), (c, v) => c + v.Position) / vertexList.Count;

            if (centroid == Vector.Zeros(3))
            {
                Debug.WriteLine("Centroid of face was the zero vector! Picking an arbitrary centroid instead");
                centroid = new Vector(new[] { 0, 0, 1.0 });
            }

            var sortedVertices = vertexList.OrderBy(vertex => vertex.Position, new ClockwiseCompare(centroid));

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
