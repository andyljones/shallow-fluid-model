using System.Collections.Generic;

namespace Assets.Views.Level.ParticleMap
{
    public class ParticleRenewalScheduler
    {
        private readonly Queue<RenewalEvent> _renewalSchedule;
        private readonly IParticleMapOptions _options;

        public ParticleRenewalScheduler(IParticleMapOptions options)
        {
            _options = options;
            _renewalSchedule = InitializeRenewalTimes(options.ParticleCount, options.ParticleLifespan);
        }

        private struct RenewalEvent
        {
            public int Time;
            public int Index;
        }

        private static Queue<RenewalEvent> InitializeRenewalTimes(int particleCount, int lifespan)
        {
            var renewalEvents = new Queue<RenewalEvent>();
            for (int i = 0; i < particleCount; i++)
            {
                var renewalEvent = new RenewalEvent {Index = i, Time = i*lifespan/particleCount};
                renewalEvents.Enqueue(renewalEvent);
            }

            return renewalEvents;
        }

        #region IndiciesToBeRenewed methods & variables
        private int _timesCalled = 0; 

        public List<int> IndicesToBeRenewed()
        {
            var lifespan = _options.ParticleLifespan;

            var indices = new List<int>(_options.ParticleCount/_options.ParticleLifespan);
            while (_renewalSchedule.Peek().Time < _timesCalled)
            {
                var renewalEvent = _renewalSchedule.Dequeue();
                indices.Add(renewalEvent.Index);

                var newRenewalEvent = new RenewalEvent
                {
                    Index = renewalEvent.Index,
                    Time = _timesCalled + lifespan
                };
                _renewalSchedule.Enqueue(newRenewalEvent);
            }

            _timesCalled = _timesCalled + 1;

            return indices;
        }
        #endregion
    }
}
