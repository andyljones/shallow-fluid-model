using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Models.MomentumModel
{
    public interface IMomentumModelParameters
    {
        double RotationFrequency { get; }
        double Gravity { get; }
        double Timestep { get; }
    }
}
