using Assets.Controllers.CursorTracker;
using Assets.Controllers.GameCamera;
using Assets.Controllers.Manipulator;
using Assets.Views.ColorMap;
using Assets.Views.LatLongGrid;
using Assets.Views.ParticleMap;
using Engine.Geometry.GeodesicSphere;
using UnityEngine;

namespace Assets.Controllers
{
    public class MainController
    {
        private readonly SimulationController _simulation;

        private readonly ColorMapView _colorMapView;
        private readonly ParticleMapView _particleMapView;

        private readonly CameraController _cameraController;
        private readonly FieldManipulator _fieldManipulator;

        public MainController(Options options)
        {
            var surface = GeodesicSphereFactory.Build(options);
            var simulation = new SimulationController(surface, options);

            var cameraController = new CameraController(options.Radius);

            var meshManager = new MeshManager(surface);
            var cursorTracker = new CursorTracker.CursorTracker(cameraController.Camera, meshManager);
            var fieldManipulator = new FieldManipulator(cursorTracker);  

            var colorMapView = new ColorMapView(surface, meshManager.Mesh, options);
            var particleMapView = new ParticleMapView(surface, options);

            LatLongGridFactory.Build(options.Radius);

            _simulation = simulation;
            _colorMapView = colorMapView;
            _particleMapView = particleMapView;
            _cameraController = cameraController;
            _fieldManipulator = fieldManipulator;
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
