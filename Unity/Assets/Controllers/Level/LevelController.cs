using System;
using Assets.Controllers.Level.Cursor;
using Assets.Controllers.Level.GameCamera;
using Assets.Controllers.Level.Manipulator;
using Assets.Controllers.Level.Simulation;
using Assets.Views.Level.ColorMap;
using Assets.Views.Level.LatLongGrid;
using Assets.Views.Level.ParticleMap;
using Assets.Views.Level.RawValues;
using Assets.Views.Level.SimulationStats;
using Engine.Geometry.GeodesicSphere;

namespace Assets.Controllers.Level
{
    public class LevelController : IDisposable
    {
        private SimulationController _simulation;
        private CameraController _cameraController;
        private FieldManipulator _fieldManipulator;
        private CursorTracker _cursorTracker;

        private ColorMapView _colorMapView;
        private ParticleMapView _particleMapView;
        private RawValuesView _rawValuesView;
        private TimeView _timeView;
        private LatLongGridView _latLongGridView;

        public LevelController(ILevelControllerOptions options)
        {
            Initialize(options);
        }
        private void Initialize(ILevelControllerOptions options)
        {
            var surface = GeodesicSphereFactory.Build(options);
            var simulation = new SimulationController(surface, options);

            var cameraController = new CameraController(options.Radius);

            var meshManager = new MeshManager(surface);
            var cursorTracker = new CursorTracker(cameraController.Camera, meshManager);
            var fieldManipulator = new FieldManipulator(surface, cursorTracker, options);

            var colorMapView = new ColorMapView(surface, meshManager.Mesh, options);
            var particleMapView = new ParticleMapView(surface, options);
            var rawValuesView = new RawValuesView(cursorTracker);
            var timeDilationView = new TimeView(50, options.Timestep);
            var latLongGridView = new LatLongGridView(options.Radius);

            _simulation = simulation;
            _colorMapView = colorMapView;
            _particleMapView = particleMapView;
            _rawValuesView = rawValuesView;
            _timeView = timeDilationView;
            _latLongGridView = latLongGridView;
            _cameraController = cameraController;
            _cursorTracker = cursorTracker;
            _fieldManipulator = fieldManipulator;
        }

        public void Update()
        {
            _simulation.Update();
            _colorMapView.Update(_simulation.CurrentFields.Height);
            _particleMapView.Update(_simulation.CurrentFields.Velocity); 
            _timeView.Update(_simulation.NumberOfSteps);

            _cameraController.Update();
            _simulation.CurrentFields.Height = _fieldManipulator.Update(_simulation.CurrentFields.Height);
        }

        public void OnGUI()
        {
            _rawValuesView.OnGUI(_simulation.CurrentFields);
            _timeView.OnGUI();
            _fieldManipulator.OnGUI();
        }

        #region IDisposable methods
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
