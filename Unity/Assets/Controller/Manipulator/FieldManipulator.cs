using System;
using Assets.Views.ColorMap;
using Engine.Geometry;
using Engine.Simulation;
using UnityEngine;

namespace Assets.Controller.Manipulator
{
    public class FieldManipulator
    {
        public KeyCode SurfaceRaiseButton = KeyCode.UpArrow;
        public KeyCode SurfaceLowerButton = KeyCode.DownArrow;

        public Func<double, double> Raise = x => x + 0.001;
        public Func<double, double> Lower = x => x - 0.001; 

        private readonly Camera _camera;
        private readonly Func<int, Face> _faceAtTriangleIndex; 

        public FieldManipulator(Camera camera, ColorMapView colorMap)
        {
            _camera = camera;
            _faceAtTriangleIndex = colorMap.MeshManager.FaceAtTriangleIndex;
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
            var face = GetFaceUnderCursor();
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

        private Face GetFaceUnderCursor()
        {
            var ray = _camera.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                var indexOfHitTriangle = hit.triangleIndex;
                var face = _faceAtTriangleIndex(indexOfHitTriangle);
                return face;
            }

            return null;
        }
    }
}
