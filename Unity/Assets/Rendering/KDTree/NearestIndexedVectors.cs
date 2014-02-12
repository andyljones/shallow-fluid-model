using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Rendering.KDTree
{
    public class NearestIndexedVectors
    {
        public List<IndexedVector> IndexedVectors { get; private set; }

        public float GreatestDistance { get { return _greatestDistance; }}
        private float _greatestDistance;

        private List<float> _distancesFromTarget;
        private readonly Vector3 _target;

        public NearestIndexedVectors(Vector3 target, int numberRequested)
        {
            _target = target;

            IndexedVectors = Enumerable.Repeat(new IndexedVector(new Vector3(), 0), numberRequested).ToList();
            _distancesFromTarget = Enumerable.Repeat(float.MaxValue, numberRequested).ToList();
        }

        public void TryAdd(IndexedVector newIndexedVector)
        {
            var newVectorsDistanceFromTarget = (newIndexedVector.Vector - _target).magnitude;
            for (int i = 0; i < IndexedVectors.Count; i++)
            {
                if (newVectorsDistanceFromTarget <= _distancesFromTarget[i])
                {
                    IndexedVectors[i] = newIndexedVector;
                    _distancesFromTarget[i] = newVectorsDistanceFromTarget;
                    _greatestDistance = _distancesFromTarget.Max();
                    return;
                }
            }
        }

        public void TryAdd(IEnumerable<IndexedVector> newVectors)
        {
            foreach (var newVector in newVectors)
            {
                TryAdd(newVector);
            }
        }
    }
}
