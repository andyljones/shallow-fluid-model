using Assets.Collider;
using Assets.Rendering;
using Assets.UserInterface;
using Engine;
using Engine.Models;
using Engine.Models.MomentumModel;
using Engine.Polyhedra;
using Engine.Polyhedra.IcosahedronBased;
using UnityEngine;

namespace Assets
{
    public class TestHook : MonoBehaviour
    {
        private IPolyhedron _polyhedron;

        private PolyhedronRenderer _polyhedronRenderer;
        private PrognosticFieldsUpdater _updater;

        private CameraPositionController _cameraPositionController;
        private PrognosticFieldsFactory _fieldFactory;
        private PrognosticFields _fields;
        private PolyhedronCollider _polyhedronCollider;
        private FieldManipulator _fieldManipulator;

        // Use this for initialization
        void Start ()
        {
            var options = new Options
            {
                MinimumNumberOfFaces = 400,
                Radius = 6000,
                SurfaceMaterialName = "Materials/Surface",
                WireframeMaterialName = "Materials/Wireframe"
            };

            _polyhedron = GeodesicSphereFactory.Build(options);

            var polyhedronMesh = new PolyhedronMeshHandler(_polyhedron);

            var polyhedronGameObject = new GameObject("Polyhedron");
            _polyhedronRenderer = new PolyhedronRenderer(polyhedronGameObject, _polyhedron, polyhedronMesh.Mesh, options);
            _polyhedronCollider = new PolyhedronCollider(polyhedronGameObject, polyhedronMesh.Mesh);

            var cameraObject = CameraObjectFactory.Build();
            _cameraPositionController = new CameraPositionController(9000, cameraObject);

            _fieldManipulator = new FieldManipulator(cameraObject.GetComponent<Camera>(), polyhedronMesh);

            _fieldFactory = new PrognosticFieldsFactory(_polyhedron);
            _fieldFactory.Height = _fieldFactory.ConstantScalarField(10);
            _fields = _fieldFactory.Build();

        }

        void Update()
        {
            _fields.Height = _fieldManipulator.Update(_fields.Height);
            _polyhedronRenderer.Update(_fields);
        }

        void LateUpdate()
        {
            _cameraPositionController.LateUpdate();   
        }
    }
}
