using System;
using UnityEngine;

namespace Assets.Controllers.Level.Manipulator
{
    /// <summary>
    /// Manages the width/intensity settings of the manipulator.
    /// </summary>
    public class FieldManipulatorSettings
    {
        public double AdjustmentSize { get; private set; }
        public int AdjustmentRadius { get; private set; }

        private readonly IFieldManipulatorOptions _options;

        /// <summary>
        /// Initialize the manipulator's width & intensity with the provided defaults.
        /// </summary>
        /// <param name="options"></param>
        public FieldManipulatorSettings(IFieldManipulatorOptions options)
        {
            _options = options;

            AdjustmentSize = 0.1;
            AdjustmentRadius = 3;
        }

        /// <summary>
        /// Servant for Unity's Update() function. Changes the manipulator's intensity & width in response to user 
        /// inputs.
        /// </summary>
        public void Update()
        {
            UpdateIntensity();
            UpdateRadius();
        }

        // Changes the manipulator's intensity in response to user inputs.
        private void UpdateIntensity()
        {
            if (Input.GetKeyDown(_options.IncreaseManipulatorMagnitudeKey))
            {
                AdjustmentSize = 10 * AdjustmentSize;
            }
            else if (Input.GetKeyDown(_options.DecreaseManipulatorMagnitudeKey))
            {
                AdjustmentSize = Math.Max(0.1 * AdjustmentSize, 0.001);
            }
        }

        // Changes the manipulator's intensity in response to user inputs.
        private void UpdateRadius()
        {
            if (Input.GetKeyDown(_options.IncreaseManipulatorRadiusKey))
            {
                AdjustmentRadius = AdjustmentRadius + 1;
            }
            else if (Input.GetKeyDown(_options.ReduceManipulatorRadiusKey))
            {
                AdjustmentRadius = Math.Max(AdjustmentRadius - 1, 1);
            }
        }

        /// <summary>
        /// Servant for Unity's OnGUI() function. Displays the current manipulator settings.
        /// </summary>
        public void OnGUI()
        {
            var style = new GUIStyle { normal = new GUIStyleState { textColor = Color.black } };

            var labelText = String.Format("Manipulator Magnitude: {0:F0}m\nManipulator Radius: {1:N0} cells", 1000*AdjustmentSize, AdjustmentRadius);
            GUI.Label(new Rect(10, Screen.height - 50, 200, 40), labelText, style);
        }
    }
}
