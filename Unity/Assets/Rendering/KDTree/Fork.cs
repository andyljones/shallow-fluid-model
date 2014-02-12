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
        private Vector3 _vector;

        public Fork(IndexedVector indexedVector, int axis, INode lesserChild, INode equalOrGreaterChild)
        {
            Axis = axis;
            IndexedVector = indexedVector;
            _vector = IndexedVector.Vector;
            LesserChild = lesserChild;
            EqualOrGreaterChild = equalOrGreaterChild;
        }

        public void TryImprove(Vector3 target, ref NearestIndexedVectors currentBests)
        {
            var targetIsLessThanKeyAlongAxis = target[Axis] < _vector[Axis];
            INode closerChild;
            INode furtherChild;
            if (targetIsLessThanKeyAlongAxis)
            {
                closerChild = LesserChild;
                furtherChild = EqualOrGreaterChild;
            }
            else
            {
                closerChild = EqualOrGreaterChild;
                furtherChild = LesserChild;
            }
            
            if (closerChild != null)
            {
                closerChild.TryImprove(target, ref currentBests);
            }

            if (Mathf.Abs(_vector[Axis] - target[Axis]) < currentBests.GreatestDistance)
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
