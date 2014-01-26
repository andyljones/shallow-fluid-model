using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Engine.Polyhedra
{
    public class Face
    {
        public readonly List<Vertex> Vertices;

        public Face(IEnumerable<Vertex> vertices)
        {
            Vertices = vertices.ToList();
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
