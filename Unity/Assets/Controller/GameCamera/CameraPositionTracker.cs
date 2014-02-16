using Assets.Views;
using UnityEngine;

namespace Assets.Controller.GameCamera
{
    public class CameraPositionTracker
    {
        public float AngularSpeed = 0.05F;
        public float RadialSpeed = 2000.0F;

        public KeyCode DragButton = KeyCode.Mouse0;

        private float _azimuth;
        private float _colatitude;
        private float _radius;

        private readonly Transform _cameraTransform;

        private const float MinColatitude = 0.001f;

        public CameraPositionTracker(float initialRadius, Transform cameraTransform)
        {
            _radius = initialRadius;
            _azimuth = 0;
            _colatitude = Mathf.PI/2;

            _cameraTransform = cameraTransform;

        }

        public void Update()
        {
            if (Input.GetKey(DragButton))
            {
                UpdateAzimuthAndColatitude();
            }

            _radius = _radius + RadialSpeed*Input.GetAxis("Mouse ScrollWheel");

            var position = GraphicsUtilities.Vector3(_colatitude, _azimuth, _radius);
            var localEast = Vector3.Cross(position, new Vector3(0, 0, 1));
            var localNorth = Vector3.Cross(localEast, position);

            _cameraTransform.rotation = Quaternion.LookRotation(-position, localNorth);
            _cameraTransform.position = position;
        }

        private void UpdateAzimuthAndColatitude()
        {
            var changeInColatitude = AngularSpeed*Input.GetAxis("Mouse Y");
            var changeInAzimuth = -AngularSpeed*Input.GetAxis("Mouse X");

            _colatitude = Mathf.Clamp(_colatitude + changeInColatitude, MinColatitude, Mathf.PI - MinColatitude);
            _azimuth = Mod(_azimuth + changeInAzimuth, 2*Mathf.PI);
        }

        private static float Mod(float x, float m)
        {
            return (x%m + m)%m;
        }
    }

}
