using System;
using System.Threading;
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
        private readonly Thread _simulationThread;
        private readonly ManualResetEvent _pauseSimulation;
        private readonly ManualResetEvent _simulationIsPaused;
        private readonly SimulationRunner _stepper;
        private bool _terminationRequested = false;

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

            _pauseSimulation = new ManualResetEvent(false);
            _simulationIsPaused = new ManualResetEvent(false);
            _simulationThread = new Thread(SimulationLoop);
            _simulationThread.Start();
        }

        // Main simulation loop. Runs the simulation and updates CurrentFields/NumberOfSteps until a pause is requested.
        private void SimulationLoop()
        {
            while (true)
            {
                // If a pause has been requested, pause the simulation, announce it's been paused, and then wait 
                // for the pause monitor to reset.
                if (!_pauseSimulation.WaitOne(0))
                {
                    _simulationIsPaused.Set();
                    _pauseSimulation.WaitOne();
                    _simulationIsPaused.Reset();
                }

                _stepper.StepSimulation();
                NumberOfSteps = NumberOfSteps + 1;
                
                _currentFieldsCache = _stepper.CurrentFields;

                // If termination has been requested, exit the loop. 
                if (_terminationRequested)
                {
                    return;
                }
            }
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
                _pauseSimulation.Reset();
                _simulationIsPaused.WaitOne();
                NumberOfSteps = 0;             
                _stepper.Reset();
                _currentFieldsCache = _stepper.CurrentFields;
            }
        }

        private void TogglePause()
        {
            if (_pauseSimulation.WaitOne(0))
            {
                _pauseSimulation.Reset();
            }
            else
            {
                _pauseSimulation.Set();
            }
        }

        /// <summary>
        /// Servant for Unity's OnGUI() function. Displays a message when the simulation is paused.
        /// </summary>
        public void OnGUI()
        {
            if (!_pauseSimulation.WaitOne(0))
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
            _terminationRequested = true;
        }
        #endregion
    }
}
