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
        public readonly Dictionary<T, int> Index;

        public double this[int i] { get { return Values[i]; } }
        public double this[T t] { get { return Values[Index[t]]; } }

        public int Count { get { return Values.Length; } }

        /// <summary>
        /// Initialize a field from a list of faces. 
        /// </summary>
        public ScalarField(IEnumerable<T> items)
        {
            Index = GenerateIndices(items);
            Values = new double[Index.Count];
        }

        /// <summary>
        /// Copies another scalar field's keys into a new field with the specified values.
        /// </summary>
        public ScalarField(Dictionary<T, int> index , double[] values)
        {
            Index = index;
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
            return new ScalarField<T>(a.Index, newValues);
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
            return new ScalarField<T>(a.Index, newValues);        
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
            return new ScalarField<T>(a.Index, newValues);
        }

        public IEnumerator<double> GetEnumerator()
        {
            return Values.ToList().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
