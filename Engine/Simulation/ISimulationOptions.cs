namespace Engine.Simulation
{
    public interface ISimulationOptions
    {
        double RotationFrequency { get; }
        double Gravity { get; }
        double Timestep { get; }
    }
}
