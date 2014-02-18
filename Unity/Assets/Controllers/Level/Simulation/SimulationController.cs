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
        private readonly SimulationStepper _stepper;

        private readonly ISimulationControllerOptions _options;

        public SimulationController(IPolyhedron surface, ISimulationControllerOptions options)
        {
            _options = options;

            var initialFields = InitialFieldsFactory.Build(surface, options);
            _stepper = new SimulationStepper(surface, initialFields, options);
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

        #region IDisposable methods
        public void Dispose()
        {
            _pauseSimulation.Reset();
            _simulationThread.Abort();
        }
        #endregion
    }
}
