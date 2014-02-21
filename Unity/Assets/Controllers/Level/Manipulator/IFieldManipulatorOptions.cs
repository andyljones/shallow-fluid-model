using UnityEngine;

namespace Assets.Controllers.Level.Manipulator
{
    public interface IFieldManipulatorOptions
    {
        KeyCode RaiseSurfaceToolKey { get; }
        KeyCode LowerSurfaceToolKey { get; }
        KeyCode IncreaseManipulatorRadiusKey { get; }
        KeyCode ReduceManipulatorRadiusKey { get; }
        KeyCode IncreaseManipulatorMagnitudeKey { get; }
        KeyCode DecreaseManipulatorMagnitudeKey { get; }
    }
}
