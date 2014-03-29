using System.Linq;

namespace Assets.Views.Level.ColorMap
{
    /// <summary>
    /// Maintains a running average of the extrema of a set of values.
    /// </summary>
    public class MaxAndMinTracker
    {
        /// <summary>
        /// Current running average of the maximum.
        /// </summary>
        public double AverageMax { get; private set; }
        /// <summary>
        /// Current running average of the minimum.
        /// </summary>
        public double AverageMin { get; private set; }
        
        // The values contributing to the running totals are maintained in arrays which are cyclically indexed into.
        // So if the (_maxes.length - 1)th value is replaced, the next value to be replaced will be the 0th value.
        private double[] _maxes;
        private double[] _mins;
        private int _index;
        
        private bool _isFirstUpdate = true;

        private readonly int _lengthOfHistory;

        /// <summary>
        /// Construct a tracker that'll average over the specified number of submitted value sets.
        /// </summary>
        /// <param name="lengthOfHistory"></param>
        public MaxAndMinTracker(int lengthOfHistory)
        {
            _lengthOfHistory = lengthOfHistory;
        }

        /// <summary>
        /// Add a new set of values to the running average.
        /// </summary>
        /// <param name="values"></param>
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

        // Construct arrays the length of the history for both maximum and minimum. Fill each with copies of the first
        // minimum and maximum respectively.
        private void InitializeMaxesAndMins(double newMax, double newMin)
        {
            _maxes = Enumerable.Repeat(newMax, _lengthOfHistory).ToArray();
            AverageMax = _maxes.Average();

            _mins = Enumerable.Repeat(newMin, _lengthOfHistory).ToArray();
            AverageMin = _mins.Average();

            // Increment the index into the array so the next submission will replace second entry in the array.
            IncrementIndex();
        }

        // Inserta new maximum and minimum into the array at the correct cyclic index, and update the averages.
        private void UpdateMaxesAndMins(double newMax, double newMin)
        {
            var oldMax = _maxes[_index];
            _maxes[_index] = newMax;
            //TODO: Rather than computing it here, replace with a lazy getter?
            AverageMax = AverageMax + (newMax - oldMax) / _lengthOfHistory;

            var oldmin = _mins[_index];
            _mins[_index] = newMin;
            AverageMin = AverageMin + (newMin - oldmin) / _lengthOfHistory;

            IncrementIndex();
        }

        // Cyclically increment the current index into the maxima & minima arrays.
        private void IncrementIndex()
        {
            _index = (_index + 1) % _lengthOfHistory;
        }
    }
}
