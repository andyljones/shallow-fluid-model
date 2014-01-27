using System;
using System.Collections.Generic;
using Engine.Polyhedra;
using MathNet.Numerics.LinearAlgebra;

namespace Engine.Simulation
{
    public class ScalarField : Dictionary<Face, Vector>
    {
        public static ScalarField operator +(ScalarField a, ScalarField b)
        {
            throw new NotImplementedException();
        }

        public static ScalarField operator -(ScalarField a, ScalarField b)
        {
            throw new NotImplementedException();            
        }

        public static ScalarField operator *(double c, ScalarField a)
        {
            throw new NotImplementedException();
        }
    }
}
