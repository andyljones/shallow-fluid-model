using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
