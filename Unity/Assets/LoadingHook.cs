using Assets.Controllers.Level;
using Assets.Controllers.Options;
using UnityEngine;

namespace Assets
{
    public class LoadingHook : MonoBehaviour
    {
        private LevelController _levelController;
        private OptionsController _optionsController;

        public void Start()
        {
            _optionsController = new OptionsController(ResetLevel);
            _levelController = new LevelController(_optionsController.Options);
        }

        private void ResetLevel()
        {
            _levelController.Dispose();
            _levelController = new LevelController(_optionsController.Options);
        }

        public void Update()
        {
            _levelController.Update();
            _optionsController.Update();
        }

        public void OnGUI()
        {
            _levelController.OnGUI();
            _optionsController.OnGUI();
        }

        void OnApplicationQuit()
        {
            _levelController.Dispose();
        }
    }
}
