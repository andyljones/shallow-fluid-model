using System;
using Assets.Rendering;
using Engine.Models;
using Engine.Polyhedra;
using UnityEngine;

namespace Assets.UserInterface
{
    public class FieldManipulator
    {
        public KeyCode SurfaceRaiseButton = KeyCode.UpArrow;
        public KeyCode SurfaceLowerButton = KeyCode.DownArrow;

        public Func<double, double> Raise = x => 1.01*x;
        public Func<double, double> Lower = x => 0.99*x; 

        private readonly Camera _camera;
        private PolyhedronMeshHandler _meshHandler;

        public FieldManipulator(Camera camera, PolyhedronMeshHandler meshHandler)
        {
            _camera = camera;
            _meshHandler = meshHandler;
        }

        public ScalarField<Face> Update(ScalarField<Face> field)
        {
            if (Input.GetKey(SurfaceRaiseButton))
            {
                Debug.Log("hi");
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
                var face = _meshHandler.FaceAtTriangleIndex(indexOfHitTriangle);
                return face;
            }

            return null;
        }
    }
}
