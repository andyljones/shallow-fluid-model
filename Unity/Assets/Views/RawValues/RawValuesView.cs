using System;
using Assets.Controllers.Cursor;
using Engine.Simulation;
using UnityEngine;

namespace Assets.Views.RawValues
{
    public class RawValuesView
    {
        private readonly CursorTracker _cursorTracker;

        public RawValuesView(CursorTracker cursorTracker)
        {
            _cursorTracker = cursorTracker;
        }

        public void UpdateGUI(PrognosticFields fields)
        {
            var height = UpdateHeightAtCursor(fields);
            var speed = UpdateSpeedAtCursor(fields);

            var labelText = String.Format("Height: {0:N0}m\nSpeed: {1:N1}kph", 1000*height, 3600*speed);

            GUI.Label(new Rect(10, 10, 200, 40), labelText);
        }

        private double? UpdateHeightAtCursor(PrognosticFields fields)
        {
            double? heightAtCursor;
            var faceUnderCursor = _cursorTracker.TryGetFaceUnderCursor();
            if (faceUnderCursor != null)
            {
                heightAtCursor = fields.Height[faceUnderCursor];
            }
            else
            {
                heightAtCursor = null;
            }

            return heightAtCursor;
        }

        private double? UpdateSpeedAtCursor(PrognosticFields fields)
        {
            double? speedAtCursor;
            var vertexUnderCursor = _cursorTracker.TryGetVertexUnderCursor();
            if (vertexUnderCursor != null)
            {
                speedAtCursor = fields.Velocity[vertexUnderCursor].Norm();
            }
            else
            {
                speedAtCursor = null;
            }

            return speedAtCursor;
        }
    }
}
