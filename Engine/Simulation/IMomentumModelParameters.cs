namespace Engine.Simulation
{
    public interface IMomentumModelParameters
    {
        double RotationFrequency { get; }
        double Gravity { get; }
        double Timestep { get; }
    }
}
