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
            _optionsController = new OptionsController();
            _levelController = new LevelController(_optionsController.Options);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                _levelController.Dispose();
                _levelController = new LevelController(_optionsController.Options);
            }
            else
            {
                _levelController.Update();
                _optionsController.Update();
            }
        }

        void OnGUI()
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
