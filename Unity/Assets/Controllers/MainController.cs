using Assets.Controllers.Cursor;
using Assets.Controllers.GameCamera;
using Assets.Controllers.Manipulator;
using Assets.Views.ColorMap;
using Assets.Views.LatLongGrid;
using Assets.Views.ParticleMap;
using Assets.Views.RawValues;
using Assets.Views.SimulationSpeed;
using Engine.Geometry.GeodesicSphere;
using UnityEngine;

namespace Assets.Controllers
{
    public class MainController
    {
        private readonly SimulationController _simulation;

        private readonly ColorMapView _colorMapView;
        private readonly ParticleMapView _particleMapView;
        private readonly RawValuesView _rawValuesView;
        private readonly TimeDilationView _timeDilationView;

        private readonly CameraController _cameraController;
        private readonly FieldManipulator _fieldManipulator;

        public MainController(Options options)
        {
            var surface = GeodesicSphereFactory.Build(options);
            var simulation = new SimulationController(surface, options);

            var cameraController = new CameraController(options.Radius);

            var meshManager = new MeshManager(surface);
            var cursorTracker = new CursorTracker(cameraController.Camera, meshManager);
            var fieldManipulator = new FieldManipulator(surface, cursorTracker);  

            var colorMapView = new ColorMapView(surface, meshManager.Mesh, options);
            var particleMapView = new ParticleMapView(surface, options);
            var rawValuesView = new RawValuesView(cursorTracker);
            var timeDilationView = new TimeDilationView(50, options.Timestep);

            LatLongGridFactory.Build(options.Radius);

            _simulation = simulation;
            _colorMapView = colorMapView;
            _particleMapView = particleMapView;
            _rawValuesView = rawValuesView;
            _timeDilationView = timeDilationView;
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
            _timeDilationView.Update(_simulation.NumberOfSteps);

            _cameraController.Update();
            _simulation.CurrentFields.Height = _fieldManipulator.Update(_simulation.CurrentFields.Height);
        }

        public void UpdateGUI()
        {
            
            _rawValuesView.UpdateGUI(_simulation.CurrentFields);
            _timeDilationView.UpdateGUI();
            _fieldManipulator.UpdateGUI();
        }

        public void Terminate()
        {
            _simulation.Terminate();
        }
    }
}
