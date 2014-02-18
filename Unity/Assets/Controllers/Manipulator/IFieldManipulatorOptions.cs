using UnityEngine;

namespace Assets.Controllers.Manipulator
{
    public interface IFieldManipulatorOptions
    {
        KeyCode SurfaceRaiseKey { get; }
        KeyCode SurfaceLowerKey { get; }
        KeyCode RadiusIncreaseKey { get; }
        KeyCode RadiusDecreaseKey { get; }
        KeyCode IntensityIncreaseKey { get; }
        KeyCode IntensityDecreaseKey { get; }
    }
}
