using System;
using System.Threading;
using Engine.Geometry;

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

        public SimulationRunner(IPolyhedron surface, PrognosticFields initialFields, ISimulationOptions options)
        {
            _fieldUpdater = new PrognosticFieldsUpdater(surface, options);
            CurrentFields = initialFields;

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
