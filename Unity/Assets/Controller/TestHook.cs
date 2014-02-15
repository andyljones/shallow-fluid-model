using Assets.Controller.UserInterface;
using Assets.Views.ColorMap;
using Assets.Views.LatLongGrid;
using Assets.Views.ParticleMap;
using Assets.Views.VectorMap;
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
        private IPolyhedron _polyhedron;

        private ColorMapGameObjectManager _colorMapGameObjectManager;

        private CameraPositionController _cameraPositionController;
        private FieldManipulator _fieldManipulator;

        private ParticleMap _compositeParticleMap;
        private SimulationRunner _simulation;
        private ColorMapView _colorMapView;


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

                InitialHeightFunction = ScalarFieldFactory.RandomScalarField,
                InitialAverageHeight = 8,
                InitialMaxDeviationOfHeight = 0.01,

                InitialVelocityFunction = VectorFieldFactory.ConstantVectorField,
                InitialAverageVelocity = Vector.Zeros(3),
                InitialMaxDeviationOfVelocity = 0,

                SurfaceMaterialName = "Materials/Surface",
                WireframeMaterialName = "Materials/Wireframe",
                ParticleMaterialName = "Materials/ParticleMap",

                ParticleCount = 20000,
                ParticleSpeedScaleFactor = 2000,
                ParticleLifespan = 1000,
                ParticleTrailLifespan = 10,
            };


            _polyhedron = GeodesicSphereFactory.Build(options);
            _colorMapView = new ColorMapView(_polyhedron, options);

            var cameraObject = CameraGameObjectFactory.Build();
            _cameraPositionController = new CameraPositionController(9000, cameraObject);

            _fieldManipulator = new FieldManipulator(cameraObject.GetComponent<Camera>(), _polyhedron);

            _simulation = new SimulationRunner(_polyhedron, options);

            _compositeParticleMap = new ParticleMap(_polyhedron, options);

            LatLongGridDrawer.DrawGrid(1.005f*(float)options.Radius);
        }

        void Update()
        {
            _simulation.CurrentFields.Height = _fieldManipulator.Update(_simulation.CurrentFields.Height);
            _colorMapView.Update(_simulation.CurrentFields.Height);
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
