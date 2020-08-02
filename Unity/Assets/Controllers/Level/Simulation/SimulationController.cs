using System;
using Engine.Geometry;
using Engine.Simulation;
using Engine.Simulation.Initialization;
using UnityEngine;

namespace Assets.Controllers.Level.Simulation
{
    /// <summary>
    /// Wraps the simulation, allowing it to be run on its own thread. The current fields of the simulation are 
    /// exposed, allowing other components to interact with it.
    /// </summary>
    public class SimulationController : IDisposable
    {
        //TODO: While nothing seems to be going horribly wrong, this isn't thread safe. Torn reads & writes are probably happening all the time.
        /// <summary>
        /// Current field values of the simulation.
        /// </summary>
        public PrognosticFields CurrentFields 
        { 
            get { return _currentFieldsCache; } 
            set { _stepper.CurrentFields = value; }
        }
        private PrognosticFields _currentFieldsCache;

        /// <summary>
        /// Number of simulation steps that have passed since it started.
        /// </summary>
        public int NumberOfSteps { get; private set; }

        // Simulation thread management variables.
        private bool _simulationIsPaused;
        private readonly SimulationRunner _stepper;

        private readonly ISimulationControllerOptions _options;

        /// <summary>
        /// Construct and start a simulation on the given surface using the given options.
        /// </summary>
        /// <param name="surface"></param>
        /// <param name="options"></param>
        public SimulationController(IPolyhedron surface, ISimulationControllerOptions options)
        {
            _options = options;

            var initialFields = InitialFieldsFactory.Build(surface, options);
            _stepper = new SimulationRunner(surface, initialFields, options);
            _currentFieldsCache = _stepper.CurrentFields;

            _simulationIsPaused = false;
        }

        /// <summary>
        /// Servant for Unity's Update() function. Allows the simulation to be paused & reset.
        /// </summary>
        public void Update()
        {

            if (Input.GetKeyDown(_options.PauseSimulationKey))
            {
                TogglePause();
            }
            else if (Input.GetKeyDown(_options.ResetSimulationKey))
            {
                // If a reset is requested, pause the simulation, reset the stepper and update the fields cache to the 
                // reset values.
                _simulationIsPaused = false;
                NumberOfSteps = 0;             
                _stepper.Reset();
                _currentFieldsCache = _stepper.CurrentFields;
            }

            if (!_simulationIsPaused) {
                _stepper.StepSimulation();
                NumberOfSteps = NumberOfSteps + 1;
                
                _currentFieldsCache = _stepper.CurrentFields;
            }

        }

        private void TogglePause()
        {
            _simulationIsPaused = !_simulationIsPaused;
        }

        /// <summary>
        /// Servant for Unity's OnGUI() function. Displays a message when the simulation is paused.
        /// </summary>
        public void OnGUI()
        {
            if (_simulationIsPaused)
            {
                var pauseMessage = String.Format("SIMULATION PAUSED\n({0} TO RESUME)", _options.PauseSimulationKey);

                var centerStyle = new GUIStyle { alignment = TextAnchor.UpperCenter, normal = new GUIStyleState { textColor = Color.black } }; ;
                GUI.Label(new Rect(Screen.width/2 - 130, 10, 260, 20), pauseMessage, centerStyle);
            }
        }

        #region IDisposable methods
        /// <summary>
        /// Terminates the simulation thread.
        /// </summary>
        public void Dispose()
        {
        }
        #endregion
    }
}
