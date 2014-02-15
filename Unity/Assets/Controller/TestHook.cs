using Assets.Controller.UserInterface;
using Assets.Views.LatLongGrid;
using Assets.Views.ParticleMap;
using Assets.Views.Surface;
using Assets.Views.VectorMap;
using Engine.GeodesicSphere;
using Engine.Geometry;
using Engine.Simulation;
using UnityEngine;

namespace Assets.Controller
{
    public class TestHook : MonoBehaviour
    {
        private IPolyhedron _polyhedron;

        private PolyhedronRenderer _polyhedronRenderer;

        private CameraPositionController _cameraPositionController;
        private PrognosticFieldsFactory _fieldFactory;
        private FieldManipulator _fieldManipulator;

        private ParticleMap _compositeParticleMap;
        private SimulationRunner _simulation;


        // Use this for initialization
        void Start ()
        {
            var options = new Options
            {
                MinimumNumberOfFaces = 400,
                Radius = 6000,

                Gravity = 10.0 / 1000.0,
                RotationFrequency = 1.0 / (3600.0*24.0),
                Timestep = 200,

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
            var polyhedronCollider = new PolyhedronCollider(polyhedronGameObject, polyhedronMesh.Mesh);

            var cameraObject = CameraObjectFactory.Build();
            _cameraPositionController = new CameraPositionController(9000, cameraObject);

            _fieldManipulator = new FieldManipulator(cameraObject.GetComponent<Camera>(), polyhedronMesh);

            _fieldFactory = new PrognosticFieldsFactory(_polyhedron);
            _fieldFactory.Height = _fieldFactory.RandomScalarField(8, 0.01);
            var initialFields = _fieldFactory.Build();

            _simulation = new SimulationRunner(_polyhedron, initialFields, options);

            var vectorFieldRenderer = new VectorFieldRenderer(_polyhedron, "VF", "Materials/Vectors");

            _compositeParticleMap = new ParticleMap(_polyhedron, options);

            LatLongGridDrawer.DrawGrid(1.005f*(float)options.Radius);
        }

        void Update()
        {
            _simulation.CurrentFields.Height = _fieldManipulator.Update(_simulation.CurrentFields.Height);
            _polyhedronRenderer.Update(_simulation.CurrentFields);
            //_vectorFieldRenderer.Update(_fields.Velocity);
            _compositeParticleMap.Update(_simulation.CurrentFields.Velocity);

            if (Input.GetKeyDown(KeyCode.R))
            {
                _simulation.TogglePause();
            }
        }

        void LateUpdate()
        {
            _cameraPositionController.LateUpdate();   
        }

        void OnApplicationQuit()
        {
            _simulation.Terminate();
        }
    }
}
