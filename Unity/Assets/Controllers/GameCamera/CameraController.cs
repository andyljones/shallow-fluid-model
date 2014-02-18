using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Controllers.GameCamera
{
    public class CameraController : IDisposable
    {
        public Camera Camera { get; private set; }

        private readonly CameraPositionTracker _positionTracker;
        private readonly GameObject _cameraGameObject;

        private const float NearClipPlane = 1;
        private const float FarClipPane = 100000;
        private const float InitialDistanceScaleFactor = 1.5f;

        public CameraController(double radius)
        {
            _cameraGameObject = CreateCamera();
            Camera = _cameraGameObject.GetComponent<Camera>();

            var intitialDistance = (float)(InitialDistanceScaleFactor*radius);
            _positionTracker = new CameraPositionTracker(intitialDistance, _cameraGameObject.transform);
        }

        public static GameObject CreateCamera()
        {
            var cameraGameObject = new GameObject("Camera");
            var camera = cameraGameObject.AddComponent<Camera>();

            camera.nearClipPlane = NearClipPlane;
            camera.farClipPlane = FarClipPane;
            camera.backgroundColor = Color.black;

            return cameraGameObject;
        }

        public void Update()
        {
            _positionTracker.Update();
        }

        #region Destructor & IDisposable methods
        public void Dispose()
        {
            Object.Destroy(_cameraGameObject);
        }
        #endregion
    }
}
