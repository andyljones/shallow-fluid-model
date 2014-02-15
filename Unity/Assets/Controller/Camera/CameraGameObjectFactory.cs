using UnityEngine;

namespace Assets.Controller.UserInterface
{
    public static class CameraGameObjectFactory
    {
        public static float NearClipPlane = 1;
        public static float FarClipPane = 100000;

        public static GameObject Build()
        {
            var cameraGameObject = new GameObject("Camera");
            var camera = cameraGameObject.AddComponent<Camera>();

            camera.nearClipPlane = NearClipPlane;
            camera.farClipPlane = FarClipPane;
            camera.backgroundColor = Color.black;

            return cameraGameObject;
        }
    }
}
