namespace Engine.Models
{
    public class SimulationParameters
    {
        public double RotationFrequency { get; set; }
        public double Gravity { get; set; }
        public double Timestep { get; set; }
        public int NumberOfRelaxationIterations { get; set; } 
    }
}
