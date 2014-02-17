using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Views.SimulationSpeed
{
    public class TimeDilationView
    {
        private List<int> _stepCountHistory;
        private List<float> _updateTimeHistory;

        private readonly int _sizeOfAveragingWindow;
        private readonly float _timestep;

        public TimeDilationView(int sizeOfAveragingWindow, double timestep)
        {
            _sizeOfAveragingWindow = sizeOfAveragingWindow;
            _timestep = (float) timestep;

            _stepCountHistory = new List<int>(sizeOfAveragingWindow+1);
            _updateTimeHistory = new List<float>(sizeOfAveragingWindow+1);
        }

        public void Update(int numberOfSteps)
        {
            var time = Time.timeSinceLevelLoad;

            _stepCountHistory.Add(numberOfSteps);
            _updateTimeHistory.Add(time);

            if (_stepCountHistory.Count > _sizeOfAveragingWindow)
            {
                _stepCountHistory = _stepCountHistory.Skip(1).ToList();
                _updateTimeHistory = _updateTimeHistory.Skip(1).ToList();
            }
        }

        public void UpdateGUI()
        {
            if (_stepCountHistory.Count >= 2)
            {
                var changeInNumberOfSteps = _stepCountHistory.Last() - _stepCountHistory.First();
                var changeInTime = _updateTimeHistory.Last() - _updateTimeHistory.First();

                var timeDilation = _timestep*changeInNumberOfSteps/changeInTime;

                var labelText = String.Format("Time Dilation: {0:G5}", timeDilation);

                GUI.Label(new Rect(Screen.width - 210, 10, 200, 20), labelText);
            }
        }
    }
}
