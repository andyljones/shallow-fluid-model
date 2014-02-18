using Assets.Controllers;
using UnityEngine;

namespace Assets
{
    public class LoadingHook : MonoBehaviour
    {
        private MainController _main;

        public void Start()
        {
            _main = new MainController();
        }

        void Update()
        {
            _main.Update();
        }

        void OnGUI()
        {
            _main.UpdateGUI();
        }

        void OnApplicationQuit()
        {
            _main.Dispose();
        }
    }
}
