using System;
using UnityEngine;

namespace Assets.Controllers.Level.Manipulator
{
    public class FieldManipulatorSettings
    {
        public double AdjustmentSize { get; private set; }
        public int AdjustmentRadius { get; private set; }

        private readonly IFieldManipulatorOptions _options;

        public FieldManipulatorSettings(IFieldManipulatorOptions options)
        {
            _options = options;

            AdjustmentSize = 0.1;
            AdjustmentRadius = 3;
        }

        public void Update()
        {
            UpdateIntensity();
            UpdateRadius();
        }

        private void UpdateIntensity()
        {
            if (Input.GetKeyDown(_options.IntensityIncreaseKey))
            {
                AdjustmentSize = 10 * AdjustmentSize;
            }
            else if (Input.GetKeyDown(_options.IntensityDecreaseKey))
            {
                AdjustmentSize = Math.Max(0.1 * AdjustmentSize, 0.001);
            }
        }

        private void UpdateRadius()
        {
            if (Input.GetKeyDown(_options.RadiusIncreaseKey))
            {
                AdjustmentRadius = AdjustmentRadius + 1;
            }
            else if (Input.GetKeyDown(_options.RadiusDecreaseKey))
            {
                AdjustmentRadius = Math.Max(AdjustmentRadius - 1, 1);
            }
        }

        public void OnGUI()
        {
            var labelText = String.Format("Adjustment Size: {0:F0}m\nAdjustment Radius: {1:N0} cells", 1000*AdjustmentSize, AdjustmentRadius);
            GUI.Label(new Rect(10, Screen.height - 50, 200, 40), labelText);
        }
    }
}
