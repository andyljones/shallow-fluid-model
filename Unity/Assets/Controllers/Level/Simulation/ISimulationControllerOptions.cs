using Engine.Simulation;
using UnityEngine;

namespace Assets.Controllers.Level.Simulation
{
    public interface ISimulationControllerOptions : ISimulationOptions
    {
        KeyCode PauseSimulationKey { get; } 
    }
}
