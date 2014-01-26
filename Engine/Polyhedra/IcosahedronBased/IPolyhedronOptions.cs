using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Polyhedra
{
    public interface IPolyhedronOptions
    {
        double Radius { get; }
        int MinimumNumberOfFaces { get; }
    }
}
