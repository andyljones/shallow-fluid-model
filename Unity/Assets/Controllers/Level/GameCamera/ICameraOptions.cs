using UnityEngine;

namespace Assets.Controllers.Level.GameCamera
{
    /// <summary>
    /// The options required by the CameraController.
    /// </summary>
    public interface ICameraOptions
    {
        double Radius { get; }

        float RadialSpeedSensitivity { get; }

        KeyCode RotateKey { get; }
        KeyCode ZoomKey { get; }
    }
}
