using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Rendering.KDTree
{
    public class KdTree
    {
        public readonly INode Root;

        public KdTree(IEnumerable<Vector3> vectors)
        {
            var vectorKeyList = BuildVectorKeyList(vectors);
            Root = BuildTree(vectorKeyList, 0);
        }

        private static List<IndexedVector> BuildVectorKeyList(IEnumerable<Vector3> vectors)
        {
            return vectors.Select((v, i) => new IndexedVector(v, i)).ToList();
        }

        public static INode BuildTree(List<IndexedVector> vectors, int depth)
        {
            if (vectors.Count <= 1)
            {
                return MakeLeaf(vectors);
            }
            else
            {
                return MakeFork(vectors, depth);
            }
        }

        private static INode MakeFork(List<IndexedVector> vectorKeys, int depth)
        {
            var axis = depth % 3;
            var medianVectorKeyAlongAxis = MedianAlongAxis(vectorKeys, axis);
            var medianVectorAlongAxis = medianVectorKeyAlongAxis.Vector;

            var vectorsLessThanMedian = new List<IndexedVector>();
            var vectorsEqualOrGreaterThanMedian = new List<IndexedVector>();

            foreach (var vectorKey in vectorKeys)
            {
                var vector = vectorKey.Vector;
                if (vector[axis] < medianVectorAlongAxis[axis] && vector != medianVectorAlongAxis)
                {
                    vectorsLessThanMedian.Add(vectorKey);
                }
                else if (vector[axis] >= medianVectorAlongAxis[axis] && vector != medianVectorAlongAxis)
                {
                    vectorsEqualOrGreaterThanMedian.Add(vectorKey);
                }
            }

            var leftSubtree = BuildTree(vectorsLessThanMedian, depth + 1);
            var rightSubtree = BuildTree(vectorsEqualOrGreaterThanMedian, depth + 1);

            var root = new Fork(medianVectorKeyAlongAxis, axis, leftSubtree, rightSubtree);

            return root;
        }

        private static IndexedVector MedianAlongAxis(List<IndexedVector> vectors, int axis)
        {
            var orderedVectors = vectors.OrderBy(vectorKey => vectorKey.Vector[axis]).ToList();

            return orderedVectors[orderedVectors.Count/2];
        }

        private static INode MakeLeaf(List<IndexedVector> vectors)
        {
            if (vectors.Count == 1)
            {
                return new Leaf(vectors.First());
            }
            else
            {
                return null;                       
            }
        }

        public List<int> GetIndicesOfNearestVectors(Vector3 target, int numberOfVectors)
        {
            var currentBests = new NearestIndexedVectors(target, numberOfVectors);
            Root.TryImprove(target, ref currentBests);

            var indices = currentBests.IndexedVectors.Select(indexedVector => indexedVector.Index).ToList();

            return indices;
        }

    }
}
