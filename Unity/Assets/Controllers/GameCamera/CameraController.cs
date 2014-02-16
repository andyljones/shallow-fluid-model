using UnityEngine;

namespace Assets.Controllers.GameCamera
{
    public class CameraController
    {
        public Camera Camera { get; private set; }

        private readonly CameraPositionTracker _positionTracker;

        private const float NearClipPlane = 1;
        private const float FarClipPane = 100000;
        private const float InitialDistanceScaleFactor = 1.5f;

        public CameraController(double radius)
        {
            var cameraGameObject = CreateCamera();
            Camera = cameraGameObject.GetComponent<Camera>();

            var intitialDistance = (float)(InitialDistanceScaleFactor*radius);
            _positionTracker = new CameraPositionTracker(intitialDistance, cameraGameObject.transform);
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
    }
}
