using Assets.Views;
using Assets.Views.Level;
using UnityEngine;

namespace Assets.Controllers.Level.GameCamera
{
    //TODO: Either come up with a better name, seeing as this doesn't just track the camera, or fold it into CameraController.
    /// <summary>
    /// Manages the camera's position and causes it to orbit the origin in response to user input.
    /// </summary>
    public class CameraPositionTracker
    {
        private const float _angularSpeed = 0.05F;
        private readonly float _radialSpeed;

        //TODO: This state is effectively replicated from _cameraTransform. It's not performance critical, so there's no real need to cache it.
        private float _azimuth;
        private float _colatitude;
        private float _radius;

        private readonly Transform _cameraTransform;
        private readonly float _surfaceRadius;

        private const float MinColatitude = 0.001f;
        private const float InitialDistanceScaleFactor = 2.5f;

        private readonly ICameraOptions _options;

        /// <summary>
        /// Constructs a new camera position tracker, using the transform of the camera to manipulate its position.
        /// </summary>
        /// <param name="cameraTransform"></param>
        /// <param name="options"></param>
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

        /// <summary>
        /// Updates the camera's radial and orbital position in response to user input.
        /// </summary>
        public void Update()
        {
            // If the rotate key is depressed, update the _azimuth and _colatitude fields.
            if (Input.GetKey(_options.RotateKey))
            {
                UpdateAzimuthAndColatitude();
            }
            // If the zoom key is depressed, update the _radius field.
            if (Input.GetKey(_options.ZoomKey))
            {
                UpdateRadius(true);
            }
            UpdateRadius(false);

            // Update the camera's transform to match the _azimuth, _colatitude & _radius fields
            var position = GraphicsUtilities.Vector3(_colatitude, _azimuth, _radius);
            var localEast = Vector3.Cross(position, new Vector3(0, 0, 1));
            var localNorth = Vector3.Cross(localEast, position);

            _cameraTransform.rotation = Quaternion.LookRotation(-position, localNorth);
            _cameraTransform.position = position;
        }

        // Update the orbital fields in response to user input.
        private void UpdateAzimuthAndColatitude()
        {
            var changeInColatitude = _angularSpeed*Input.GetAxis("Mouse Y");
            var changeInAzimuth = -_angularSpeed*Input.GetAxis("Mouse X");

            _colatitude = Mathf.Clamp(_colatitude + changeInColatitude, MinColatitude, Mathf.PI - MinColatitude);
            _azimuth = Mod(_azimuth + changeInAzimuth, 2*Mathf.PI);
        }

        // Update the radial field in response to user input.
        private void UpdateRadius(bool keyed)
        {
            if (keyed) {
                _radius = Mathf.Max(_surfaceRadius, _radius + _radialSpeed * -Input.GetAxis("Mouse Y"));
            } else {
                _radius = Mathf.Max(_surfaceRadius, _radius + 3*_radialSpeed * -Input.GetAxis("Mouse ScrollWheel"));
            }
        }

        // Calculate x modulo m (which has a range [0, m)). This is distinct from x%m, which is x remainder m 
        // (and so has a range (-m, m)).
        private static float Mod(float x, float m)
        {
            return (x%m + m)%m;
        }
    }

}
