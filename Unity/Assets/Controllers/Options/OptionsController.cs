using System;
using UnityEngine;

namespace Assets.Controllers.Options
{
    public class OptionsController
    {
        public GameOptions Options { get; private set; }
        private GameOptions _currentOptions;

        private readonly Action _resetLevel;

        private bool _optionsMenuIsOpen = false;

        public OptionsController(Action resetLevel)
        {
            _resetLevel = resetLevel;
            _currentOptions = InitialOptionsFactory.Build();
            Options = _currentOptions.Copy();
        }

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

        public void OnGUI()
        {
            if (_optionsMenuIsOpen)
            {
                DrawOptionsMenu();
            }
        }

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

        private static void LabelAndNumericField(string label, ref double value)
        {
            GUILayout.BeginHorizontal();

                GUILayout.Label(label, GUILayout.Width(295));
                double.TryParse(GUILayout.TextField(value.ToString(), GUILayout.Width(100)), out value);

            GUILayout.EndHorizontal();
        }

        private static void LabelAndNumericField(string label, ref int value)
        {
            GUILayout.BeginHorizontal();

                GUILayout.Label(label, GUILayout.Width(295));
                int.TryParse(GUILayout.TextField(value.ToString(), GUILayout.Width(100)), out value);

            GUILayout.EndHorizontal();
        }

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
