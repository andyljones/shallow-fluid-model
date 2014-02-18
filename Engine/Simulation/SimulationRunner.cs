using Engine.Geometry;

namespace Engine.Simulation
{
    public class SimulationRunner
    {
        public PrognosticFields CurrentFields;
        private readonly PrognosticFields _initialFields;

        private PrognosticFields _oldFields;
        private PrognosticFields _olderFields;

        private readonly PrognosticFieldsUpdater _fieldUpdater;

        public SimulationRunner(IPolyhedron surface, PrognosticFields initialFields, IModelParameters options)
        {
            _fieldUpdater = new PrognosticFieldsUpdater(surface, options);
            _initialFields = initialFields;
            CurrentFields = initialFields;
        }

        public void Reset()
        {
            _olderFields = null;
            _oldFields = null;
            CurrentFields = _initialFields;
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
