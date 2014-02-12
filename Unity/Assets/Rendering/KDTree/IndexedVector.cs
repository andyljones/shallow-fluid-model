using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Rendering.KDTree
{
    public class IndexedVector
    {
        public readonly Vector3 Vector;
        public readonly int Index;

        public IndexedVector(Vector3 vector, int index)
        {
            Vector = vector;
            Index = index;
        }
    }
}
