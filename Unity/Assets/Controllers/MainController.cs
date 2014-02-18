using System;
using Assets.Controllers.Cursor;
using Assets.Controllers.GameCamera;
using Assets.Controllers.Manipulator;
using Assets.Controllers.Simulation;
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
        private SimulationController _simulation;

        private ColorMapView _colorMapView;
        private ParticleMapView _particleMapView;
        private RawValuesView _rawValuesView;
        private TimeDilationView _timeDilationView;
        private LatLongGridView _latLongGridView;
                
        private CameraController _cameraController;
        private FieldManipulator _fieldManipulator;
        private CursorTracker _cursorTracker;

        private IMainControllerOptions _options;

        public MainController(IMainControllerOptions options)
        {
            Initialize(options);
        }

        private void Initialize(IMainControllerOptions options)
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

        public void Reset(IMainControllerOptions options = null)
        {
            if (options == null)
            {
                options = _options;
            }

            Dispose();
            Initialize(options);
        }

        public void Update()
        {
            if (Input.GetKeyDown(_options.ResetKey))
            {
                Reset();
            }

            _simulation.Update();
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
