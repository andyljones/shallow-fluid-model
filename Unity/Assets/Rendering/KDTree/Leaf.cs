using UnityEngine;

namespace Assets.Rendering.KDTree
{
    public class Leaf : INode
    {
        public readonly IndexedVector Key;

        public Leaf(IndexedVector key)
        {
            Key = key;
        }

        public void TryImprove(Vector3 target, ref NearestIndexedVectors currentBests)
        {
            currentBests.TryAdd(Key);
        }
    }
}
