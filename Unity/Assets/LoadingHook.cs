using Assets.Controllers.Level;
using Assets.Controllers.Options;
using Assets.Views.Options;
using UnityEngine;

namespace Assets
{
    /// <summary>
    /// Loads the top-level controllers into the game environment.
    /// </summary>
    public class LoadingHook : MonoBehaviour
    {
        private LevelController _levelController;
        private OptionsController _optionsController;
        private HelpView _helpView;

        /// <summary>
        /// Called on level load by the Unity engine. Initializes and links together the top-level controllers.
        /// </summary>
        public void Start()
        {
            _optionsController = new OptionsController(ResetLevel);
            _levelController = new LevelController(_optionsController.Options);
            _helpView = new HelpView(_optionsController.Options);
        }

        /// <summary>
        /// Resets the level by destroying the current level controller and creating a new one with the current set of 
        /// options
        /// </summary>
        private void ResetLevel()
        {
            _levelController.Dispose();
            _levelController = new LevelController(_optionsController.Options);
        }

        /// <summary>
        /// Called once a frame by the Unity engine. Captures control input and displays 3D graphics.
        /// </summary>
        public void Update()
        {
            _levelController.Update();
            _optionsController.Update();
            _helpView.Update();
        }

        /// <summary>
        /// Called several times per in-game frame by the Unity engine in order to provide a smooth GUI experience.
        /// </summary>
        public void OnGUI()
        {
            _levelController.OnGUI();
            _optionsController.OnGUI();
            _helpView.OnGUI();
        }

        /// <summary>
        /// Called by the Unity engine when the application is closed. Cleans up level controller & its simulation thread.
        /// </summary>
        public void OnApplicationQuit()
        {
            _levelController.Dispose();
        }
    }
}
