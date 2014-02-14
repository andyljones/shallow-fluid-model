using System;
using UnityEngine;

namespace Assets.UserInterface
{
    public class CameraPositionController
    {
        public float AngularSpeed = 0.05F;
        public float RadialSpeed = 2000.0F;

        public KeyCode DragButton = KeyCode.Mouse0;

        private float _azimuth;
        private float _colatitude;
        private float _radius;

        private Transform _cameraTransform;

        public CameraPositionController(float initialRadius, GameObject camera)
        {
            _radius = initialRadius;
            _azimuth = 0;
            _colatitude = Mathf.PI/2;

            _cameraTransform = camera.transform;
        }

        public void LateUpdate()
        {
            if (Input.GetKey(DragButton))
            {
                _azimuth = MathMod(_azimuth - AngularSpeed*Input.GetAxis("Mouse X"), 2*Mathf.PI);
                _colatitude = Mathf.Clamp(_colatitude + AngularSpeed*Input.GetAxis("Mouse Y"), 0.001f, Mathf.PI-0.01f);
            }

            _radius += Input.GetAxis("Mouse ScrollWheel") * RadialSpeed;

            var x = _radius*Mathf.Sin(_azimuth)*Mathf.Sin(_colatitude);
            var y = _radius*Mathf.Cos(_azimuth)*Mathf.Sin(_colatitude);
            var z = _radius*Mathf.Cos(_colatitude);

            var position = new Vector3(x, y, z);
            var localEast = Vector3.Cross(position, new Vector3(0, 0, 1));
            var localNorth = Vector3.Cross(localEast, position);

            _cameraTransform.rotation = Quaternion.LookRotation(-position, localNorth);
            _cameraTransform.position = position;
        }

        private float MathMod(float x, float m)
        {
            return (x%m + m)%m;
        }
    }

}
