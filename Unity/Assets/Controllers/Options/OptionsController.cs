using System;
using Assets.Controllers.Level;
using UnityEngine;

namespace Assets.Controllers.Options
{
    public class OptionsController
    {
        public GameOptions Options;

        private bool _optionsMenuIsOpen = false;

        public OptionsController()
        {
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
            GUI.Box(new Rect(100, 100, Screen.width - 200, Screen.width - 200), "Options");
        }
    }
}
