using Engine.Geometry;

namespace Engine.Simulation
{
    /// <summary>
    /// Manages the initialization & running of the simulation. 
    /// </summary>
    public class SimulationRunner
    {
        public PrognosticFields CurrentFields;
        private readonly PrognosticFields _initialFields;

        private PrognosticFields _oldFields;
        private PrognosticFields _olderFields;

        private readonly PrognosticFieldsUpdater _fieldUpdater;

        /// <summary>
        /// Creates a simulation on the given surface using the given initial fields & options.
        /// </summary>
        /// <param name="surface"></param>
        /// <param name="initialFields"></param>
        /// <param name="options"></param>
        public SimulationRunner(IPolyhedron surface, PrognosticFields initialFields, IModelParameters options)
        {
            _fieldUpdater = new PrognosticFieldsUpdater(surface, options);
            _initialFields = initialFields;
            CurrentFields = initialFields;
        }

        /// <summary>
        /// Reset the simulation to the set of fields it was created with.
        /// </summary>
        public void Reset()
        {
            _olderFields = null;
            _oldFields = null;
            CurrentFields = _initialFields;
        }

        /// <summary>
        /// Step the simulation forward.
        /// </summary>
        public void StepSimulation()
        {
            var oldestFields = _olderFields;
            _olderFields = _oldFields;
            _oldFields = CurrentFields;
            CurrentFields = _fieldUpdater.Update(_oldFields, _olderFields, oldestFields);
        }
    }
}
