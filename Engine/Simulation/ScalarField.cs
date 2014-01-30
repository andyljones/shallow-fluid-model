using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Simulation
{
    /// <summary>
    /// Represents a scalar field over a set of items.
    /// </summary>
    public class ScalarField<T> : IEnumerable<double>
    {
        public readonly double[] Values;
        public readonly Func<T, int> IndexOf;

        public double this[int i] { get { return Values[i]; } }
        public double this[T t] { get { return Values[IndexOf(t)]; } }

        public int Count { get { return Values.Length; } }

        /// <summary>
        /// Copies another scalar field's keys into a new field with the specified values.
        /// </summary>
        public ScalarField(Func<T, int> index, double[] values)
        {
            IndexOf = index;
            Values = values;
        }

        private static Dictionary<T, int> GenerateIndices(IEnumerable<T> items)
        {
            var itemList = items.ToList();
            var indices = Enumerable.Range(0, itemList.Count);
            var dictionary = indices.ToDictionary(i => itemList[i], i => i);

            return dictionary;
        }

        /// <summary>
        /// Returns a new field containing the sum of the two fields.
        /// </summary>
        public static ScalarField<T> operator +(ScalarField<T> a, ScalarField<T> b)
        {
            var newValues = new double[a.Values.Length];
            for (int i = 0; i < a.Values.Length; i++)
            {
                newValues[i] = a.Values[i] + b.Values[i];
            }
            return new ScalarField<T>(a.IndexOf, newValues);
        }

        /// <summary>
        /// Returns a new field containing the difference of the two fields.
        /// </summary>
        public static ScalarField<T> operator -(ScalarField<T> a, ScalarField<T> b)
        {
            var newValues = new double[a.Values.Length];
            for (int i = 0; i < a.Values.Length; i++)
            {
                newValues[i] = a.Values[i] - b.Values[i];
            }
            return new ScalarField<T>(a.IndexOf, newValues);        
        }

        /// <summary>
        /// Returns the negation of the field.
        /// </summary>
        public static ScalarField<T> operator -(ScalarField<T> a)
        {
            return -1 * a;
        }

        /// <summary>
        /// Returns a new field with the constant subtracted from each element of the field.
        /// </summary>
        public static ScalarField<T> operator -(ScalarField<T> a, double b)
        {
            var newValues = new double[a.Values.Length];
            for (int i = 0; i < a.Values.Length; i++)
            {
                newValues[i] = a.Values[i] - b;
            }
            return new ScalarField<T>(a.IndexOf, newValues);
        }


        /// <summary>
        /// Returns a new field containing the field scaled by <param name="c">c</param>.
        /// </summary>
        public static ScalarField<T> operator *(double c, ScalarField<T> a)
        {
            var newValues = new double[a.Values.Length];
            for (int i = 0; i < a.Values.Length; i++)
            {
                newValues[i] = c*a.Values[i];
            }
            return new ScalarField<T>(a.IndexOf, newValues);
        }



        public IEnumerator<double> GetEnumerator()
        {
            return Values.ToList().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


        public override string ToString()
        {
            return String.Join(", " , Values.Select(value => value.ToString("N3")).ToArray());
        }
    }
}
