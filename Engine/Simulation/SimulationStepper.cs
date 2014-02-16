using System;
using System.Threading;
using Engine.Geometry;
using Engine.Simulation.Initialization;

namespace Engine.Simulation
{
    public class SimulationStepper
    {
        public PrognosticFields CurrentFields;

        private PrognosticFields _oldFields;
        private PrognosticFields _olderFields;

        private readonly PrognosticFieldsUpdater _fieldUpdater;

        public SimulationStepper(IPolyhedron surface, ISimulationOptions options)
        {
            _fieldUpdater = new PrognosticFieldsUpdater(surface, options as IModelParameters);
            CurrentFields = InitialFieldsFactory.Build(surface, options as IInitialFieldParameters);
        }

        public void StepSimulation()
        {
            var oldestFields = _olderFields;
            _olderFields = _oldFields;
            _oldFields = CurrentFields;
            CurrentFields = _fieldUpdater.Update(_oldFields, _olderFields, oldestFields);
        }
    }
}
