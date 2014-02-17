using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Controllers.Manipulator
{
    public class FieldManipulatorProperties
    {
        public KeyCode IntensityIncreaseKey = KeyCode.RightArrow;
        public KeyCode IntensityDecreaseKey = KeyCode.LeftArrow;

        public KeyCode RadiusIncreaseKey = KeyCode.RightBracket;
        public KeyCode RadiusDecreaseKey = KeyCode.LeftBracket;

        public double AdjustmentSize = .1;
        public int Radius = 1;

        public void Update()
        {
            UpdateIntensity();
            UpdateRadius();
        }

        private void UpdateIntensity()
        {
            if (Input.GetKeyDown(IntensityIncreaseKey))
            {
                AdjustmentSize = 10 * AdjustmentSize;
            }
            else if (Input.GetKeyDown(IntensityDecreaseKey))
            {
                AdjustmentSize = 0.1 * AdjustmentSize;
            }
        }

        private void UpdateRadius()
        {
            if (Input.GetKeyDown(RadiusIncreaseKey))
            {
                Radius = Radius + 1;
            }
            else if (Input.GetKeyDown(RadiusDecreaseKey))
            {
                Radius = Radius - 1;
            }
        }

        public void UpdateGUI()
        {
            var labelText = String.Format("Adjustment Size: {0:F0}m\nRadius: {1:N}", 1000*AdjustmentSize, Radius);
            GUI.Label(new Rect(10, Screen.height - 50, 200, 40), labelText);
        }
    }
}
