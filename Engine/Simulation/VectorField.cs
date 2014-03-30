using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;

namespace Engine.Simulation
{
    /// <summary>
    /// Represents a vector field on a collection of type T entities.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class VectorField<T> : IEnumerable<Vector>
    {
        public readonly Func<T, int> IndexOf;
        public readonly Vector[] Values;

        public Vector this[int i] { get { return Values[i]; } }
        public Vector this[T t] { get { return Values[IndexOf(t)]; } }

        public int Count { get { return Values.Length; } }

        /// <summary>
        /// Construct a vector field with the ith vector being associated with whichever T object is such that 
        /// indexOf(obj) == i.
        /// </summary>
        /// <param name="indexOf"></param>
        /// <param name="values"></param>
        public VectorField(Func<T, int> indexOf, Vector[] values)
        {
            IndexOf = indexOf;
            Values = values;
        }

        #region Operators
        public static VectorField<T> operator +(VectorField<T> a, VectorField<T> b)
        {
            var newValues = new Vector[a.Values.Length];
            for (int i = 0; i < a.Values.Length; i++)
            {
                newValues[i] = a.Values[i] + b.Values[i];
            }
            return new VectorField<T>(a.IndexOf, newValues);
        }

        public static VectorField<T> operator -(VectorField<T> a, VectorField<T> b)
        {
            var newValues = new Vector[a.Values.Length];
            for (int i = 0; i < a.Values.Length; i++)
            {
                newValues[i] = a.Values[i] - b.Values[i];
            }
            return new VectorField<T>(a.IndexOf, newValues);
        }

        public static VectorField<T> operator *(double c, VectorField<T> a)
        {
            var newValues = new Vector[a.Values.Length];
            for (int i = 0; i < a.Values.Length; i++)
            {
                newValues[i] = c * a.Values[i];
            }
            return new VectorField<T>(a.IndexOf, newValues);
        }

        public static VectorField<T> operator *(ScalarField<T> a, VectorField<T> b)
        {
            var newValues = new Vector[a.Values.Length];
            for (int i = 0; i < a.Values.Length; i++)
            {
                newValues[i] = a.Values[i] * b.Values[i];
            }
            return new VectorField<T>(a.IndexOf, newValues);
        }
        #endregion

        public static VectorField<T> CrossProduct(VectorField<T> a, VectorField<T> b)
        {
            var newValues = new Vector[a.Values.Length];
            for (int i = 0; i < a.Values.Length; i++)
            {
                newValues[i] = Vector.CrossProduct(a.Values[i], b.Values[i]);
            }
            return new VectorField<T>(a.IndexOf, newValues);
        }

        public static ScalarField<T> ScalarProduct(VectorField<T> a, VectorField<T> b)
        {
            var newValues = new double[a.Values.Length];
            for (int i = 0; i < a.Values.Length; i++)
            {
                newValues[i] = Vector.ScalarProduct(a.Values[i], b.Values[i]);
            }
            return new ScalarField<T>(a.IndexOf, newValues);
        }
        
        public IEnumerator<Vector> GetEnumerator()
        {
            return Values.ToList().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


        public override string ToString()
        {
            return String.Join(", ", Values.Select(value => VectorString(value)).ToArray());
        }

        private string VectorString(Vector v)
        {
            return String.Format("({0,3:N2}, {1,3:N2}, {2,3:N2})", v[0], v[1], v[2]);
        }
    }
}
