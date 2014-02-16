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

        public Func<double, double> Raise = x => x + 0.01;
        public Func<double, double> Lower = x => x - 0.01;
        
        private readonly CursorTracker _cursorTracker;

        public FieldManipulator(CursorTracker cursorTracker)
        {
            _cursorTracker = cursorTracker;
        }

        public ScalarField<Face> Update(ScalarField<Face> field)
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

        private ScalarField<Face> TryUpdateFieldUnderCursor(ScalarField<Face> field, Func<double, double> update)
        {
            var face = _cursorTracker.TryGetFaceUnderCursor();
            if (face != null)
            {
                var values = field.Values;
                values[field.IndexOf(face)] = update(field[face]);

                return new ScalarField<Face>(field.IndexOf, values);
            }
            else
            {
                return field;
            }
        }
    }
}
