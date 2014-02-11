using UnityEngine;

namespace Assets.UserInterface
{
    public static class CameraObjectFactory
    {
        public static float NearClipPlane = 1;
        public static float FarClipPane = 100000;

        public static GameObject Build()
        {
            var cameraGameObject = new GameObject("Camera");
            var camera = cameraGameObject.AddComponent<Camera>();

            camera.nearClipPlane = NearClipPlane;
            camera.farClipPlane = FarClipPane;

            return cameraGameObject;
        }
    }
}
