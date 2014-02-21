using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Assets.Controllers.Options;
using UnityEngine;

namespace Assets.Views.Options
{
    public class HelpView
    {
        private readonly GameOptions _options;

        private bool _menuIsOpen = false;
        private Dictionary<string, KeyCode> _currentControls; 

        public HelpView(GameOptions options)
        {
            _options = options;
        }

        public void Update()
        {
            if (Input.GetKeyDown(_options.HelpMenuKey))
            {
                _menuIsOpen = !_menuIsOpen;
                _currentControls = GetDictionaryOfControls(_options);
            }
        }

        public void OnGUI()
        {
            if (_menuIsOpen)
            {
                DrawHelpMenu();
            }

            DrawHelpLabel();
        }

        private void DrawHelpLabel()
        {
            var rightAlign = new GUIStyle { alignment = TextAnchor.UpperRight, normal = new GUIStyleState { textColor = Color.black } };
            var text = String.Format("Press {0} for help", _options.HelpMenuKey);
            GUI.Label(new Rect(Screen.width - 110, Screen.height - 30, 100, 20), text, rightAlign);
        }

        private static Dictionary<string, KeyCode> GetDictionaryOfControls(GameOptions gameOptions)
        {
            var properties = gameOptions.GetType().GetProperties();
            var controls = properties.Where(prop => prop.PropertyType == typeof (KeyCode));

            return controls.ToDictionary(control => control.Name, control => (KeyCode)control.GetValue(gameOptions, null));
        }

        private void DrawHelpMenu()
        {
            GUI.Box(new Rect(Screen.width / 2 - 210, 100, 420, 370), "");
            GUILayout.BeginArea(new Rect(Screen.width / 2 - 200, 107, 400, 355));

            GUILayout.BeginVertical();

            GUILayout.Box("Controls");

            foreach (var controlNameAndValue in _currentControls)
            {
                DrawControl(controlNameAndValue.Key, controlNameAndValue.Value);
            }

            DrawButton();

            GUILayout.EndVertical();

            GUILayout.EndArea();
        }

        private void DrawControl(string controlName, KeyCode controlValue)
        {
            GUILayout.BeginHorizontal();

            GUILayout.Label(ParseControlName(controlName), GUILayout.Width(220));
            GUILayout.Label(CamelCaseToSpaceSeparated(controlValue.ToString()), GUILayout.Width(200));

            GUILayout.EndHorizontal();
        }

        private static string ParseControlName(string controlName)
        {
            var spaceSeparated = CamelCaseToSpaceSeparated(controlName);

            var suffixOfKeyRegex = new Regex(@" [Kk]ey$");
            var parsedName = suffixOfKeyRegex.Replace(spaceSeparated, "");

            return parsedName;
        }

        private static string CamelCaseToSpaceSeparated(string text)
        {
            var internalCapitalLettersRegex = new Regex(@"
                (?<=[A-Z])(?=[A-Z][a-z]) |
                 (?<=[^A-Z])(?=[A-Z]) |
                 (?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace);

            var spaceSeparatedText = internalCapitalLettersRegex.Replace(text, " ");

            return spaceSeparatedText;
        }

        private void DrawButton()
        {
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Cancel"))
            {
                _menuIsOpen = false;
            }

            GUILayout.EndHorizontal();
        }
    }
}
