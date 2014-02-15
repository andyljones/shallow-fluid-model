using System;
using System.Threading;
using Engine.Geometry;
using Engine.Simulation.Initialization;

namespace Engine.Simulation
{
    public class SimulationRunner
    {
        public PrognosticFields CurrentFields;
        public int NumberOfSteps { get; private set; }

        private PrognosticFields _oldFields;
        private PrognosticFields _olderFields;

        private readonly PrognosticFieldsUpdater _fieldUpdater;

        private readonly Thread _simulationThread;
        private readonly ManualResetEvent _pauseEvent;

        public SimulationRunner(IPolyhedron surface, ISimulationOptions options)
        {
            _fieldUpdater = new PrognosticFieldsUpdater(surface, options as IModelParameters);
            CurrentFields = InitialFieldsFactory.Build(surface, options as IInitialFieldParameters);

            _pauseEvent = new ManualResetEvent(false);
            _simulationThread = new Thread(StepSimulation);
            _simulationThread.Start();
        }

        public void TogglePause()
        {
            if (_pauseEvent.WaitOne(0))
            {
                _pauseEvent.Reset();
            }
            else
            {
                _pauseEvent.Set();
            }
        }

        public void StepSimulation()
        {
            while (true)
            {
                _pauseEvent.WaitOne();

                var oldestFields = _olderFields;
                _olderFields = _oldFields;
                _oldFields = CurrentFields;
                CurrentFields = _fieldUpdater.Update(_oldFields, _olderFields, oldestFields);

                NumberOfSteps = NumberOfSteps + 1;
            }
        }

        public void Terminate()
        {
            _pauseEvent.Reset();
            _simulationThread.Abort();
        }
    }
}
