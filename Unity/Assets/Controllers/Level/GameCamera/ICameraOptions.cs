using UnityEngine;

namespace Assets.Controllers.Level.GameCamera
{
    public interface ICameraOptions
    {
        double Radius { get; }

        float RadialSpeedSensitivity { get; }

        KeyCode RotateKey { get; }
        KeyCode ZoomKey { get; }
    }
}
