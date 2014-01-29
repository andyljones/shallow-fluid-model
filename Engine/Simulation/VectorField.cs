using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;

namespace Engine.Simulation
{
    public class VectorField<T> : IEnumerable<Vector>
    {
        public readonly Dictionary<T, int> Index;
        public readonly Vector[] Values;

        public Vector this[int i] { get { return Values[i]; } }
        public Vector this[T t] { get { return Values[Index[t]]; } }

        public int Count { get { return Values.Length; } }

        public VectorField(Dictionary<T, int> index, Vector[] values)
        {
            Index = index;
            Values = values;
        }

        public IEnumerator<Vector> GetEnumerator()
        {
            return Values.ToList().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
