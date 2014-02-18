using Assets.Controllers.Level;
using Assets.Controllers.Options;
using UnityEngine;

namespace Assets
{
    public class LoadingHook : MonoBehaviour
    {
        private LevelController _level;

        public void Start()
        {
            var initialOptions = InitialOptionsFactory.Build();
            _level = new LevelController(initialOptions);
        }

        void Update()
        {
            _level.Update();
        }

        void OnGUI()
        {
            _level.UpdateGUI();
        }

        void OnApplicationQuit()
        {
            _level.Dispose();
        }
    }
}
