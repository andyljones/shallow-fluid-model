using Assets.Controllers;
using Assets.Controllers.GameCamera;
using Assets.Controllers.Manipulator;
using Assets.Views.ColorMap;
using Assets.Views.LatLongGrid;
using Assets.Views.ParticleMap;
using Engine.Geometry;
using Engine.Geometry.GeodesicSphere;
using UnityEngine;

namespace Assets.Controllers
{
    public class MainController
    {
        private IPolyhedron _polyhedron;
        private SimulationController _simulation;

        private ColorMapView _colorMapView;
        private ParticleMapView _particleMapView;

        private CameraController _cameraController;
        private FieldManipulator _fieldManipulator;

        public MainController(Options options)
        {
            InitializeModel(options);
            InitializeViews(options);
            InitializeControllers(options);
        }

        private void InitializeModel(Options options)
        {
            _polyhedron = GeodesicSphereFactory.Build(options);
            _simulation = new SimulationController(_polyhedron, options);
        }

        private void InitializeViews(Options options)
        {
            LatLongGridFactory.Build(options.Radius);

            _colorMapView = new ColorMapView(_polyhedron, options);
            _particleMapView = new ParticleMapView(_polyhedron, options);
        }

        private void InitializeControllers(Options options)
        {
            _cameraController = new CameraController(options.Radius);
            _fieldManipulator = new FieldManipulator(_cameraController.Camera, _colorMapView.MeshManager);            
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                _simulation.TogglePause();
            }

            _colorMapView.Update(_simulation.CurrentFields.Height);
            _particleMapView.Update(_simulation.CurrentFields.Velocity); 

            _cameraController.Update();
            _simulation.CurrentFields.Height = _fieldManipulator.Update(_simulation.CurrentFields.Height);
        }

        public void Terminate()
        {
            _simulation.Terminate();
        }
    }
}
