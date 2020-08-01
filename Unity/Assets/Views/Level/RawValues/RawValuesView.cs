using System;
using Assets.Controllers.Level.Cursor;
using Engine.Simulation;
using UnityEngine;

namespace Assets.Views.Level.RawValues
{
    /// <summary>
    /// Displays the raw values of the simulation field in the face under the cursor.
    /// </summary>
    public class RawValuesView
    {
        private readonly CursorTracker _cursorTracker;

        public RawValuesView(CursorTracker cursorTracker)
        {
            _cursorTracker = cursorTracker;
        }

        public void OnGUI(PrognosticFields fields)
        {
            var height = UpdateHeightAtCursor(fields);
            var speed = UpdateSpeedAtCursor(fields);

            var style = new GUIStyle {normal = new GUIStyleState {textColor = Color.black}};
            var labelText = String.Format("Height: {0:N0}m\nSpeed: {1:N1}kph", 1000*height, 3600*speed);

            GUI.Label(new Rect(10, 10, 200, 40), labelText, style);
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
                speedAtCursor = fields.Velocity[vertexUnderCursor].Norm(2);
            }
            else
            {
                speedAtCursor = null;
            }

            return speedAtCursor;
        }
    }
}
