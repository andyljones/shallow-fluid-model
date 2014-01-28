using System;

namespace Engine.Polyhedra
{
    public static class EdgeUtilities
    {
        public static double Length(this Edge edge)
        {
            return (edge.A.Position - edge.B.Position).Norm();
        }
    }
}
