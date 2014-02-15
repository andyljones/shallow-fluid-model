namespace Engine.Simulation
{
    public interface IModelParameters
    {
        double RotationFrequency { get; }
        double Gravity { get; }
        double Timestep { get; }
    }
}
