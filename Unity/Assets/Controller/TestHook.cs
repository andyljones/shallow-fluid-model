using Assets.Controller.GameCamera;
using Assets.Controller.Manipulator;
using Assets.Views.ColorMap;
using Assets.Views.LatLongGrid;
using Assets.Views.ParticleMap;
using Engine.Geometry;
using Engine.Geometry.GeodesicSphere;
using Engine.Simulation;
using Engine.Simulation.Initialization;
using MathNet.Numerics.LinearAlgebra;
using UnityEngine;

namespace Assets.Controller
{
    public class TestHook : MonoBehaviour
    {
        private ColorMapGameObjectManager _colorMapGameObjectManager;

        private CameraPositionController _cameraPositionController;
        private FieldManipulator _fieldManipulator;

        private ParticleMapView _particleMapView;
        private SimulationRunner _simulation;
        private ColorMapView _colorMapView;


        // Use this for initialization
        void Start ()
        {
            var options = new Options
            {
                MinimumNumberOfFaces = 500,
                Radius = 6000,
                
                Gravity = 10.0 / 1000.0,
                RotationFrequency = 1.0 / (3600.0*24.0),
                Timestep = 200,

                InitialHeightFunction = ScalarFieldFactory.RandomScalarField,
                InitialAverageHeight = 8,
                InitialMaxDeviationOfHeight = 0,

                InitialVelocityFunction = VectorFieldFactory.ConstantVectorField,
                InitialAverageVelocity = new Vector(new []{0, .0001, 0}),
                InitialMaxDeviationOfVelocity = 0,

                ColorMapHistoryLength = 1000,
                ColorMapMaterialName = "Materials/Surface",

                ParticleCount = 20000,
                ParticleSpeedScaleFactor = 2000,
                ParticleLifespan = 1000,
                ParticleTrailLifespan = 10,
                ParticleMaterialName = "Materials/ParticleMap",
            };


            var polyhedron = GeodesicSphereFactory.Build(options);
            _simulation = new SimulationRunner(polyhedron, options);

            var cameraObject = CameraGameObjectFactory.Build();
            _colorMapView = new ColorMapView(polyhedron, options);
            _particleMapView = new ParticleMapView(polyhedron, options);
            LatLongGridDrawer.DrawGrid(options.Radius);

            _fieldManipulator = new FieldManipulator(cameraObject.GetComponent<Camera>(), _colorMapView.MeshManager);

            _cameraPositionController = new CameraPositionController(9000, cameraObject);
            StartCoroutine(_cameraPositionController.Coroutine());
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                _simulation.TogglePause();
            }

            _simulation.CurrentFields.Height = _fieldManipulator.Update(_simulation.CurrentFields.Height);
            _colorMapView.Update(_simulation.CurrentFields.Height);
            _particleMapView.Update(_simulation.CurrentFields.Velocity); 
        }

        void OnApplicationQuit()
        {
            _simulation.Terminate();
        }
    }
}
