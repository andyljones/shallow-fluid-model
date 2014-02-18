using Engine.Simulation;
using UnityEngine;

namespace Assets.Controllers.Simulation
{
    public interface ISimulationControllerOptions : ISimulationOptions
    {
        KeyCode PauseSimulationKey { get; } 
    }
}
