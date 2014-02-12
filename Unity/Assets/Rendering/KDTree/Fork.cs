using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Rendering.KDTree
{
    public class Fork : INode
    {
        public readonly int Axis;
        public readonly IndexedVector IndexedVector;
        public readonly INode LesserChild;
        public readonly INode EqualOrGreaterChild;

        public Fork(IndexedVector indexedVector, int axis, INode lesserChild, INode equalOrGreaterChild)
        {
            Axis = axis;
            IndexedVector = indexedVector;
            LesserChild = lesserChild;
            EqualOrGreaterChild = equalOrGreaterChild;
        }

        public void TryImprove(Vector3 target, ref NearestIndexedVectors currentBests)
        {
            var targetIsLessThanKeyAlongAxis = target[Axis] < IndexedVector.Vector[Axis];
            var closerChild = targetIsLessThanKeyAlongAxis? LesserChild : EqualOrGreaterChild;
            var furtherChild = targetIsLessThanKeyAlongAxis ? EqualOrGreaterChild : LesserChild;

            if (closerChild != null)
            {
                closerChild.TryImprove(target, ref currentBests);
            }

            if (IndexedVector.Vector[Axis] - target[Axis] < currentBests.GreatestDistance())
            {
                currentBests.TryAdd(IndexedVector);

                if (furtherChild != null)
                {
                    furtherChild.TryImprove(target, ref currentBests);
                }
            }
        }
    }
}
