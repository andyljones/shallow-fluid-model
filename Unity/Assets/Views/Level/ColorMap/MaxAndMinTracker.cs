using System.Linq;

namespace Assets.Views.Level.ColorMap
{
    public class MaxAndMinTracker
    {
        public double AverageMax { get; private set; }
        public double AverageMin { get; private set; }
        
        private double[] _maxes;
        private double[] _mins;
        private int _index;
        
        private bool _isFirstUpdate = true;

        private readonly int _lengthOfHistory;

        public MaxAndMinTracker(int lengthOfHistory)
        {
            _lengthOfHistory = lengthOfHistory;
        }

        public void Update(double[] values)
        {
            var max = values.Max();
            var min = values.Min();

            if (_isFirstUpdate)
            {
                InitializeMaxesAndMins(max, min);
                _isFirstUpdate = false;
            }
            else
            {
                UpdateMaxesAndMins(max, min);
            }
        }

        private void InitializeMaxesAndMins(double newMax, double newMin)
        {
            _maxes = Enumerable.Repeat(newMax, _lengthOfHistory).ToArray();
            AverageMax = _maxes.Average();

            _mins = Enumerable.Repeat(newMin, _lengthOfHistory).ToArray();
            AverageMin = _mins.Average();

            IncrementIndex();
        }

        private void UpdateMaxesAndMins(double newMax, double newMin)
        {
            var oldMax = _maxes[_index];
            _maxes[_index] = newMax;
            AverageMax = AverageMax + (newMax - oldMax) / _lengthOfHistory;

            var oldmin = _mins[_index];
            _mins[_index] = newMin;
            AverageMin = AverageMin + (newMin - oldmin) / _lengthOfHistory;

            IncrementIndex();
        }

        private void IncrementIndex()
        {
            _index = (_index + 1) % _lengthOfHistory;
        }
    }
}
