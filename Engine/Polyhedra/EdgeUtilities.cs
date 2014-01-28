using System;
using Engine.Utilities;

namespace Engine.Polyhedra
{
    public static class EdgeUtilities
    {
        public static double Length(this Edge edge)
        {
            return VectorUtilities.GeodesicDistance(edge.A.Position, edge.B.Position);
        }
    }
}
