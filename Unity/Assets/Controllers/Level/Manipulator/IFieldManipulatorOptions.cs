using UnityEngine;

namespace Assets.Controllers.Level.Manipulator
{
    /// <summary>
    /// Options required by the FieldManipulator class.
    /// </summary>
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
