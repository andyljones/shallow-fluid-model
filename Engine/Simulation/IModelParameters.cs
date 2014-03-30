namespace Engine.Simulation
{
    /// <summary>
    /// Options required to define the simulation.
    /// </summary>
    public interface IModelParameters
    {
        double RotationFrequency { get; }
        double Gravity { get; }
        double Timestep { get; }
    }
}
