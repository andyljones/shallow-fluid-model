using System;
using System.Threading;
using Engine.Geometry;
using Engine.Simulation;
using Engine.Simulation.Initialization;
using UnityEngine;

namespace Assets.Controllers.Level.Simulation
{
    public class SimulationController : IDisposable
    {
        public PrognosticFields CurrentFields 
        { 
            get { return _currentFieldsCache; } 
            set { _stepper.CurrentFields = value; }
        }
        private PrognosticFields _currentFieldsCache;

        public int NumberOfSteps { get; private set; }

        private readonly Thread _simulationThread;
        private readonly ManualResetEvent _pauseSimulation;
        private readonly ManualResetEvent _simulationIsPaused;
        private readonly SimulationRunner _stepper;
        private bool _terminationRequested = false;

        private readonly ISimulationControllerOptions _options;

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

        private void SimulationLoop()
        {
            while (true)
            {
                if (!_pauseSimulation.WaitOne(0))
                {
                    _simulationIsPaused.Set();
                    _pauseSimulation.WaitOne();
                    _simulationIsPaused.Reset();
                }
                _stepper.StepSimulation();
                NumberOfSteps = NumberOfSteps + 1;
                
                _currentFieldsCache = _stepper.CurrentFields;

                if (_terminationRequested)
                {
                    return;
                }
            }
        }

        public void Update()
        {
            if (Input.GetKeyDown(_options.PauseSimulationKey))
            {
                TogglePause();
            }
            else if (Input.GetKeyDown(_options.ResetSimulationKey))
            {
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

        public void OnGUI()
        {
            if (!_pauseSimulation.WaitOne(0))
            {
                var pauseMessage = String.Format("SIMULATION PAUSED ({0} TO RESUME)", _options.PauseSimulationKey);

                GUI.Label(new Rect(Screen.width/2 - 130, 100, 260, 20), pauseMessage);
            }
        }


        #region IDisposable methods
        public void Dispose()
        {
            _terminationRequested = true;
        }
        #endregion
    }
}
