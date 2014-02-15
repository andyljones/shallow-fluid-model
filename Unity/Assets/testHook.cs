using System.Linq;
using Assets.Collider;
using Assets.Rendering;
using Assets.Rendering.LatLongGrid;
using Assets.Rendering.ParticleMap;
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
        private PolyhedronCollider _polyhedronCollider;
        private FieldManipulator _fieldManipulator;

        private PrognosticFields _fields;
        private PrognosticFields _oldFields;
        private PrognosticFields _olderFields;
        private VectorFieldRenderer _vectorFieldRenderer;
        private ParticleMap _compositeParticleMap;


        // Use this for initialization
        void Start ()
        {
            var options = new Options
            {
                MinimumNumberOfFaces = 500,
                Radius = 6000,

                Gravity = 10.0 / 1000.0,
                RotationFrequency = 1.0 / (3600.0*24.0),
                Timestep = 400,

                SurfaceMaterialName = "Materials/Surface",
                WireframeMaterialName = "Materials/Wireframe",
                ParticleMaterialName = "Materials/ParticleMap",

                ParticleCount = 20000,
                WindmapScaleFactor = 2000,
                ParticleLifespan = 1000,
                ParticleTrailLifespan = 10,
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
            _fieldFactory.Height = _fieldFactory.RandomScalarField(8, 0.01);
            _fields = _fieldFactory.Build();

            _updater = new PrognosticFieldsUpdater(_polyhedron, options);

            _vectorFieldRenderer = new VectorFieldRenderer(_polyhedron, "VF", "Materials/Vectors");

            _compositeParticleMap = new ParticleMap(_polyhedron, options);

            LatLongGridDrawer.DrawGrid(1.005f*(float)options.Radius);
        }

        private bool _isRunning = false;

        void Update()
        {
            _fields.Height = _fieldManipulator.Update(_fields.Height);
            _polyhedronRenderer.Update(_fields);
            //_vectorFieldRenderer.Update(_fields.Velocity);
            _compositeParticleMap.Update(_fields.Velocity);

            if (Input.GetKeyDown(KeyCode.R))
            {
                _isRunning = !_isRunning;
            }

            if (_isRunning)
            {
                var oldestFields = _olderFields;
                _olderFields = _oldFields;
                _oldFields = _fields;
                _fields = _updater.Update(_oldFields, _olderFields, oldestFields);

            }

        }

        void LateUpdate()
        {
            _cameraPositionController.LateUpdate();   
        }
    }
}
