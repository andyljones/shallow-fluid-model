using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Engine.Polyhedra
{
    public class Face
    {
        public ReadOnlyCollection<Vertex> Vertices { get; private set; }

        public Face(IEnumerable<Vertex> vertices)
        {
            Vertices = new ReadOnlyCollection<Vertex>(vertices.ToList());
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

            if (a == null || b == null)
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
            if (face == null)
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
