using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Controllers.Level.GameCamera
{
    /// <summary>
    /// Controls the camera's position.
    /// </summary>
    public class CameraController : IDisposable
    {
        /// <summary>
        /// Get the Camera object the controller is using.
        /// </summary>
        public Camera Camera { get; private set; }

        private readonly CameraPositionTracker _positionTracker;
        private readonly GameObject _cameraGameObject;

        private const float NearClipPlane = 1;
        private const float FarClipPane = 1000000;

        /// <summary>
        /// Creates a game camera and the controls to maneauver it.
        /// </summary>
        /// <param name="options"></param>
        public CameraController(ICameraOptions options)
        {
            _cameraGameObject = CreateCamera();
            Camera = _cameraGameObject.GetComponent<Camera>();

            _positionTracker = new CameraPositionTracker(_cameraGameObject.transform, options);
        }

        // Creates a Camera object.
        private static GameObject CreateCamera()
        {
            var cameraGameObject = new GameObject("Camera");
            var camera = cameraGameObject.AddComponent<Camera>();

            camera.nearClipPlane = NearClipPlane;
            camera.farClipPlane = FarClipPane;
            camera.backgroundColor = Color.white;

            return cameraGameObject;
        }

        /// <summary>
        /// Servant for Unity's Update() function, intended to be called once a frame.
        /// </summary>
        public void Update()
        {
            _positionTracker.Update();
        }

        #region Destructor & IDisposable methods
        /// <summary>
        /// Destroys the controller's Camera object.
        /// </summary>
        public void Dispose()
        {
            Object.Destroy(_cameraGameObject);
        }
        #endregion
    }
}
