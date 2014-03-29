using System;
using UnityEngine;

namespace Assets.Controllers.Options
{
    /// <summary>
    /// Allows users to change some of the app's options. Displays an options menu.
    /// </summary>
    public class OptionsController
    {
        /// <summary>
        /// The current set of options.
        /// </summary>
        public GameOptions Options { get; private set; }
        private GameOptions _currentOptions;

        private readonly Action _resetLevel;

        private bool _optionsMenuIsOpen = false;

        /// <summary>
        /// Generates an options menu that uses the specified resetLevel method to reload the level when a new set of 
        /// options are applied.
        /// </summary>
        /// <param name="resetLevel"></param>
        public OptionsController(Action resetLevel)
        {
            _resetLevel = resetLevel;
            _currentOptions = InitialOptionsFactory.Build();
            Options = _currentOptions.Copy();
        }

        /// <summary>
        /// Servant for Unity's Update() function. Opens or closes an options menu in response to user input.
        /// </summary>
        public void Update()
        {
            if (Input.GetKeyDown(Options.OptionsMenuKey))
            {
                if (!_optionsMenuIsOpen)
                {
                    _currentOptions = Options.Copy();
                }

                _optionsMenuIsOpen = !_optionsMenuIsOpen;
            }
        }

        /// <summary>
        /// Servant for Unity's OnGUI() function. Displays an options menu if it's currently open.
        /// </summary>
        public void OnGUI()
        {
            if (_optionsMenuIsOpen)
            {
                DrawOptionsMenu();
            }
        }

        // Draws an options menu.
        private void DrawOptionsMenu()
        {
            GUI.Box(new Rect(Screen.width / 2 - 210, 100, 420, 370), "");
            GUILayout.BeginArea(new Rect(Screen.width/2 - 200, 107, 400, 355));

                GUILayout.BeginVertical();

                    DrawSimulationParameters();
                    DrawInitialHeightfieldParameters();
                    DrawParticleParameters();
                    DrawButtons();

                GUILayout.EndVertical();

            GUILayout.EndArea();
        }

        // Displays the simulation parameter options of the options menu.
        private void DrawSimulationParameters()
        {
            GUILayout.Box("Simulation Parameters");

            var cellCount = _currentOptions.MinimumNumberOfFaces;
            LabelAndNumericField("Minimum Number of Cells", ref cellCount);
            _currentOptions.MinimumNumberOfFaces = cellCount;

            var radius = _currentOptions.Radius;
            LabelAndNumericField("Radius (km)", ref radius);
            _currentOptions.Radius = radius;

            var timestep = _currentOptions.Timestep;
            LabelAndNumericField("Timestep (s)", ref timestep);
            _currentOptions.Timestep = timestep;

            var gravity = 1000*_currentOptions.Gravity;
            LabelAndNumericField("Gravity (m/s^2)", ref gravity);
            _currentOptions.Gravity = gravity/1000;

            var period = 1 / _currentOptions.RotationFrequency;
            LabelAndNumericField("Rotation Period (s)", ref period);
            _currentOptions.RotationFrequency = 1 / period;
        }

        // Displays the initial parameters options of the options menu.
        private void DrawInitialHeightfieldParameters()
        {
            GUILayout.Box("Initial Heightfield Parameters");

            var averageHeight = _currentOptions.InitialAverageHeight;
            LabelAndNumericField("Average Height (km)", ref averageHeight);
            _currentOptions.InitialAverageHeight = averageHeight;

            var deviationHeight = _currentOptions.InitialMaxDeviationOfHeight;
            LabelAndNumericField("Maximum Deviation in Height (km)", ref deviationHeight);
            _currentOptions.InitialMaxDeviationOfHeight = deviationHeight;
        }

        // Displays the particle parameters options of the options menu.
        private void DrawParticleParameters()
        {
            GUILayout.Box("Particle Parameters");

            var particleCount = _currentOptions.ParticleCount;
            LabelAndNumericField("Particle Count", ref particleCount);
            _currentOptions.ParticleCount = particleCount;

            var trailLifespan = _currentOptions.ParticleTrailLifespan;
            LabelAndNumericField("Trail Lifespan (frames)", ref trailLifespan);
            _currentOptions.ParticleTrailLifespan = trailLifespan;

            var speedMultiplier = _currentOptions.ParticleSpeedScaleFactor;
            LabelAndNumericField("Particle Speed Scale Factor", ref speedMultiplier);
            _currentOptions.ParticleSpeedScaleFactor = speedMultiplier;
        }

        // Utility function that'll display a label on the left and a double-type user input box on the right.
        private static void LabelAndNumericField(string label, ref double value)
        {
            GUILayout.BeginHorizontal();

                GUILayout.Label(label, GUILayout.Width(295));
                double.TryParse(GUILayout.TextField(value.ToString(), GUILayout.Width(100)), out value);

            GUILayout.EndHorizontal();
        }

        // Utility function that'll display a label on the left and an int-type user input box on the right.
        private static void LabelAndNumericField(string label, ref int value)
        {
            GUILayout.BeginHorizontal();

                GUILayout.Label(label, GUILayout.Width(295));
                int.TryParse(GUILayout.TextField(value.ToString(), GUILayout.Width(100)), out value);

            GUILayout.EndHorizontal();
        }

        // Displays an apply & cancel button on the options menu.
        private void DrawButtons()
        {
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Rebuild Simulation"))
            {
                Options = _currentOptions.Copy();
                _optionsMenuIsOpen = false;
                _resetLevel();
            }

            if (GUILayout.Button("Cancel"))
            {
                _optionsMenuIsOpen = false;
            }

            GUILayout.EndHorizontal();
        }
    }
}
