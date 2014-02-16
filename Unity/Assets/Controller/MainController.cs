using Assets.Controller.GameCamera;
using Assets.Controller.Manipulator;
using Assets.Views.ColorMap;
using Assets.Views.LatLongGrid;
using Assets.Views.ParticleMap;
using Engine.Geometry;
using Engine.Geometry.GeodesicSphere;
using Engine.Simulation;
using UnityEngine;

namespace Assets.Controller
{
    public class MainController
    {
        private IPolyhedron _polyhedron;
        private SimulationController _simulationController;

        private ColorMapView _colorMapView;
        private ParticleMapView _particleMapView;

        private CameraController _cameraController;
        private FieldManipulator _fieldManipulator;

        public MainController(Options options)
        {
            StartModel(options);
            StartViews(options);
            StartControllers(options);
        }

        private void StartModel(Options options)
        {
            _polyhedron = GeodesicSphereFactory.Build(options);
            _simulationController = new SimulationController(_polyhedron, options);
        }

        private void StartViews(Options options)
        {
            LatLongGridFactory.Build(options.Radius);

            _colorMapView = new ColorMapView(_polyhedron, options);
            _particleMapView = new ParticleMapView(_polyhedron, options);
        }

        private void StartControllers(Options options)
        {
            _cameraController = new CameraController(options.Radius);
            _fieldManipulator = new FieldManipulator(_cameraController.Camera, _colorMapView.MeshManager);            
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                _simulationController.TogglePause();
            }

            _colorMapView.Update(_simulationController.CurrentFields.Height);
            _particleMapView.Update(_simulationController.CurrentFields.Velocity); 

            _cameraController.Update();
            _simulationController.CurrentFields.Height = _fieldManipulator.Update(_simulationController.CurrentFields.Height);
        }

        public void Terminate()
        {
            _simulationController.Terminate();
        }
    }
}
