using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Rendering.KDTree
{
    public interface INode
    {
        void TryImprove(Vector3 target, ref NearestIndexedVectors currentBests);
    }
}
