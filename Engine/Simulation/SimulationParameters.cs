using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Simulation
{
    public class SimulationParameters
    {
        public double RotationFrequency { get; set; }
        public double Gravity { get; set; }
        public double Timestep { get; set; }
        public int NumberOfRelaxationIterations { get; set; } 
    }
}
