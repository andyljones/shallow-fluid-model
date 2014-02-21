using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Controllers.Level.GameCamera
{
    public class CameraController : IDisposable
    {
        public Camera Camera { get; private set; }

        private readonly CameraPositionTracker _positionTracker;
        private readonly GameObject _cameraGameObject;

        private const float NearClipPlane = 1;
        private const float FarClipPane = 1000000;

        public CameraController(ICameraOptions options)
        {
            _cameraGameObject = CreateCamera();
            Camera = _cameraGameObject.GetComponent<Camera>();

            _positionTracker = new CameraPositionTracker(_cameraGameObject.transform, options);
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
