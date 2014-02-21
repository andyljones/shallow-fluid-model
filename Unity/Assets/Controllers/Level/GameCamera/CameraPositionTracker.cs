using Assets.Views;
using Assets.Views.Level;
using UnityEngine;

namespace Assets.Controllers.Level.GameCamera
{
    public class CameraPositionTracker
    {
        private const float _angularSpeed = 0.05F;
        private readonly float _radialSpeed;

        private float _azimuth;
        private float _colatitude;
        private float _radius;

        private readonly Transform _cameraTransform;
        private readonly float _surfaceRadius;

        private const float MinColatitude = 0.001f;
        private const float InitialDistanceScaleFactor = 1.5f;

        private readonly ICameraOptions _options;

        public CameraPositionTracker(Transform cameraTransform, ICameraOptions options)
        {
            _options = options;

            _surfaceRadius = (float)options.Radius;
            _radialSpeed = _surfaceRadius * options.RadialSpeedSensitivity;

            _radius = InitialDistanceScaleFactor * (float)options.Radius;
            _azimuth = 0;
            _colatitude = Mathf.PI/2;

            _cameraTransform = cameraTransform;

        }

        public void Update()
        {
            if (Input.GetKey(_options.RotateKey))
            {
                UpdateAzimuthAndColatitude();
            }
            if (Input.GetKey(_options.ZoomKey))
            {
                UpdateRadius();
            }

            var position = GraphicsUtilities.Vector3(_colatitude, _azimuth, _radius);
            var localEast = Vector3.Cross(position, new Vector3(0, 0, 1));
            var localNorth = Vector3.Cross(localEast, position);

            _cameraTransform.rotation = Quaternion.LookRotation(-position, localNorth);
            _cameraTransform.position = position;
        }

        private void UpdateAzimuthAndColatitude()
        {
            var changeInColatitude = _angularSpeed*Input.GetAxis("Mouse Y");
            var changeInAzimuth = -_angularSpeed*Input.GetAxis("Mouse X");

            _colatitude = Mathf.Clamp(_colatitude + changeInColatitude, MinColatitude, Mathf.PI - MinColatitude);
            _azimuth = Mod(_azimuth + changeInAzimuth, 2*Mathf.PI);
        }

        private void UpdateRadius()
        {
            _radius = Mathf.Max(_surfaceRadius, _radius + _radialSpeed * -Input.GetAxis("Mouse Y"));
        }

        private static float Mod(float x, float m)
        {
            return (x%m + m)%m;
        }
    }

}
