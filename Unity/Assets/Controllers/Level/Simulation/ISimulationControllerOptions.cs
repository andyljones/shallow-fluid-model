using Engine.Simulation;
using Engine.Simulation.Initialization;
using UnityEngine;

namespace Assets.Controllers.Level.Simulation
{
    /// <summary>
    /// Options required by the SimulationController class.
    /// </summary>
    public interface ISimulationControllerOptions : IModelParameters, IInitialFieldParameters
    {
        KeyCode PauseSimulationKey { get; }
        KeyCode ResetSimulationKey { get; }
    }
}
