using System;
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
    public class MainController : IDisposable
    {
        private readonly SimulationController _simulation;

        private readonly ColorMapView _colorMapView;
        private readonly ParticleMapView _particleMapView;
        private readonly RawValuesView _rawValuesView;
        private readonly TimeDilationView _timeDilationView;
        private readonly LatLongGridView _latLongGridView;

        private readonly CameraController _cameraController;
        private readonly FieldManipulator _fieldManipulator;
        private readonly CursorTracker _cursorTracker;

        private readonly IMainControllerOptions _options;

        public MainController(IMainControllerOptions options)
        {
            _options = options;

            var surface = GeodesicSphereFactory.Build(options);
            var simulation = new SimulationController(surface, options);

            var cameraController = new CameraController(options.Radius);

            var meshManager = new MeshManager(surface);
            var cursorTracker = new CursorTracker(cameraController.Camera, meshManager);
            var fieldManipulator = new FieldManipulator(surface, cursorTracker, options);  

            var colorMapView = new ColorMapView(surface, meshManager.Mesh, options);
            var particleMapView = new ParticleMapView(surface, options);
            var rawValuesView = new RawValuesView(cursorTracker);
            var timeDilationView = new TimeDilationView(50, options.Timestep);
            var latLongGridView = new LatLongGridView(options.Radius);

            _simulation = simulation;
            _colorMapView = colorMapView;
            _particleMapView = particleMapView;
            _rawValuesView = rawValuesView;
            _timeDilationView = timeDilationView;
            _latLongGridView = latLongGridView;
            _cameraController = cameraController;
            _cursorTracker = cursorTracker;
            _fieldManipulator = fieldManipulator;
        }

        public void Update()
        {
            if (Input.GetKeyDown(_options.PauseSimulationKey))
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

        #region Destructor & IDisposable methods
        public void Dispose()
        {
            _simulation.Dispose();
            _cursorTracker.Dispose();
            _cameraController.Dispose();
            _colorMapView.Dispose();
            _particleMapView.Dispose();
            _latLongGridView.Dispose();
        }
        #endregion
    }
}
