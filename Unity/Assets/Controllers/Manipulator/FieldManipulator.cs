using System;
using Assets.Controllers.Cursor;
using Assets.Views.ColorMap;
using Engine.Geometry;
using Engine.Simulation;
using UnityEngine;

namespace Assets.Controllers.Manipulator
{
    public class FieldManipulator
    {
        public KeyCode SurfaceRaiseButton = KeyCode.UpArrow;
        public KeyCode SurfaceLowerButton = KeyCode.DownArrow;

        public KeyCode IntensityIncreaseButton = KeyCode.RightArrow;
        public KeyCode IntensityDecreaseButton = KeyCode.LeftArrow;

        private double _adjustmentSize = .1;

        private readonly CursorTracker _cursorTracker;

        public FieldManipulator(CursorTracker cursorTracker)
        {
            _cursorTracker = cursorTracker;
        }

        public ScalarField<Face> Update(ScalarField<Face> field)
        {
            CheckForIntensityAdjustment();
            
            return AdjustedField(field);
        }

        private void CheckForIntensityAdjustment()
        {
            if (Input.GetKeyDown(IntensityIncreaseButton))
            {
                _adjustmentSize = 10 * _adjustmentSize;
            }
            else if (Input.GetKeyDown(IntensityDecreaseButton))
            {
                _adjustmentSize = 0.1 * _adjustmentSize;
            }
        }

        private ScalarField<Face> AdjustedField(ScalarField<Face> field)
        {
            if (Input.GetKey(SurfaceRaiseButton))
            {
                return TryUpdateFieldUnderCursor(field, Raise);
            }
            else if (Input.GetKey(SurfaceLowerButton))
            {
                return TryUpdateFieldUnderCursor(field, Lower);
            }
            else
            {
                return field;
            }
        }

        public void UpdateGUI()
        {
            var labelText = String.Format("Adjustment Size: {0:F0}m", 1000*_adjustmentSize);
            GUI.Label(new Rect(10, Screen.height - 50, 200, 20), labelText);
        }

        private ScalarField<Face> TryUpdateFieldUnderCursor(ScalarField<Face> field, Func<double, double> update)
        {
            var face = _cursorTracker.TryGetFaceUnderCursor();
            if (face != null)
            {
                //TODO: This modifies the old values!
                var values = field.Values;
                values[field.IndexOf(face)] = update(field[face]);

                return new ScalarField<Face>(field.IndexOf, values);
            }
            else
            {
                return field;
            }
        }

        private double Raise(double x)
        {
            return x + _adjustmentSize;
        }

        private double Lower(double x)
        {
            return x - _adjustmentSize;
        }
    }
}
