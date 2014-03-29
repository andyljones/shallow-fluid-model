using Engine.Simulation;
using UnityEngine;

namespace Assets.Controllers.Level.Simulation
{
    /// <summary>
    /// Options required by the SimulationController class.
    /// </summary>
    public interface ISimulationControllerOptions : ISimulationOptions
    {
        KeyCode PauseSimulationKey { get; }
        KeyCode ResetSimulationKey { get; }
    }
}
