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
    /// <summary>
    /// Controls in-scene processes and entities. In particular, it manages
    ///  - The simulation
    ///  - The visualization of the simulation, inc the heatmap, particles and time display.
    ///  - Any interaction with the simulation
    ///  - The camera position
    ///  - The cursor's position
    /// </summary>
    public class LevelController : IDisposable
    {
        // Controls the simulation thread, which does all the number crunching.
        private SimulationController _simulationController;
        
        // Controls interactions with the simulation.
        private FieldManipulator _fieldManipulator;

        // Tracks the cursor's position, which is necessary for interaction with the simulation and for display of the 
        // values of the simulation cell underneath the cursor.
        private CursorTracker _cursorTracker;

        // Controls the camera's movement.
        private CameraController _cameraController;
        

        // Displays the heatmap which reflects the current simulation heightfield
        private ColorMapView _colorMapView;

        // Displays the particles which follow the simulation's vector field
        private ParticleMapView _particleMapView;

        // Displays the values of the simulation cell underneath the cursor's position
        private RawValuesView _rawValuesView;

        // Displays the current in-simulation time and time-since-start
        private TimeView _timeView;

        // Displays the static latitude-longitude grid.
        private LatLongGridView _latLongGridView;

        /// <summary>
        /// Creates a simulation with the specified options, along with all the additional structures needed to display 
        /// and interact with it.
        /// </summary>
        /// <param name="options">The options to use in creating the simulation</param>
        public LevelController(ILevelControllerOptions options)
        {
            Initialize(options);
        }
        
        private void Initialize(ILevelControllerOptions options)
        {
            // Constructs all the in-level components, then stores the ones that'll be needed later in member variables.
            // Done this way rather than directly initializing the member variables because this way if they're reordered,
            // the compiler will complain if something's being constructed before its dependency.
            var surface = GeodesicSphereFactory.Build(options);
            var simulation = new SimulationController(surface, options);

            var cameraController = new CameraController(options);

            var meshManager = new MeshManager(surface);
            var cursorTracker = new CursorTracker(cameraController.Camera, meshManager);
            var fieldManipulator = new FieldManipulator(surface, cursorTracker, options);

            var colorMapView = new ColorMapView(surface, meshManager.Mesh, options);
            var particleMapView = new ParticleMapView(surface, options);
            var rawValuesView = new RawValuesView(cursorTracker);
            var timeDilationView = new TimeView(50, options.Timestep);
            var latLongGridView = new LatLongGridView(options.Radius);

            _simulationController = simulation;
            _colorMapView = colorMapView;
            _particleMapView = particleMapView;
            _rawValuesView = rawValuesView;
            _timeView = timeDilationView;
            _latLongGridView = latLongGridView;
            _cameraController = cameraController;
            _cursorTracker = cursorTracker;
            _fieldManipulator = fieldManipulator;
        }

        /// <summary>
        /// Servant for the Unity engine's Update() function. Intended to be called once a frame.
        /// </summary>
        public void Update()
        {
            _simulationController.Update();
            _colorMapView.Update(_simulationController.CurrentFields.Height);
            _particleMapView.Update(_simulationController.CurrentFields.Velocity); 
            _timeView.Update(_simulationController.NumberOfSteps);

            _cameraController.Update();
            _simulationController.CurrentFields.Height = _fieldManipulator.Update(_simulationController.CurrentFields.Height);
        }

        /// <summary>
        /// Servant for the Unity engine's OnGUI() function. Intended to be called several times a frame.
        /// </summary>
        public void OnGUI()
        {
            _simulationController.OnGUI();            
            _rawValuesView.OnGUI(_simulationController.CurrentFields);
            _timeView.OnGUI();
            _fieldManipulator.OnGUI();
        }

        #region IDisposable methods
        /// <summary>
        /// Tears down the LevelController in a safe manner. In particular it safely disposes of the simulation's thread.
        /// After it executes, it should be safe to instantiate another LevelController in its place.
        /// </summary>
        public void Dispose()
        {
            _simulationController.Dispose();
            _cursorTracker.Dispose();
            _cameraController.Dispose();
            _colorMapView.Dispose();
            _particleMapView.Dispose();
            _latLongGridView.Dispose();
        }
        #endregion
    }
}
