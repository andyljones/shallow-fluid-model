using System;
using UnityEngine;

namespace Assets.Controllers.Options
{
    public class OptionsController
    {
        public GameOptions Options { get; private set; }
        private GameObject _currentOptions;

        private readonly Action _resetLevel;

        private bool _optionsMenuIsOpen = false;

        public OptionsController(Action resetLevel)
        {
            _resetLevel = resetLevel;
            Options = InitialOptionsFactory.Build();
        }

        public void Update()
        {
            if (Input.GetKeyDown(Options.OptionsMenuKey))
            {
                _optionsMenuIsOpen = !_optionsMenuIsOpen;
            }
        }

        public void OnGUI()
        {
            if (_optionsMenuIsOpen)
            {
                DrawOptionsMenu();
            }
        }

        private void DrawOptionsMenu()
        {
            var groupRect = new Rect(100, 100, Screen.width - 200, Screen.height - 200);
            GUI.BeginGroup(groupRect);
            GUI.Box(new Rect(0, 0, Screen.width - 200, Screen.height - 200), "Options");

            if (GUI.Button(new Rect(Screen.width/2 - 150, 30, 100, 20), "Reset"))
            {
                _resetLevel();
                _optionsMenuIsOpen = false;
            }
            
            GUI.EndGroup();
        }
    }
}
