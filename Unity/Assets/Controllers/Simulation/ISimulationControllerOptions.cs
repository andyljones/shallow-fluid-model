using Engine.Simulation;
using UnityEngine;

namespace Assets.Controllers
{
    public interface ISimulationControllerOptions : ISimulationOptions
    {
        KeyCode PauseSimulationKey { get; } 
    }
}
