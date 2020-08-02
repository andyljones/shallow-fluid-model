using Engine.Simulation.Initialization;
using MathNet.Numerics.LinearAlgebra;
using UnityEngine;

namespace Assets.Controllers.Options
{
    using Vector = Vector<double>;

    /// <summary>
    /// Factory for construction a default GameOptions object.
    /// </summary>
    public static class InitialOptionsFactory
    {
        private static readonly GameOptions InitialOptions = new GameOptions
        {
            MinimumNumberOfFaces = 100,
            Radius = 6000,

            Gravity = 10.0 / 1000.0,
            RotationFrequency = 1.0 / (3600.0 * 24.0),
            Timestep = 400,

            InitialHeightFunction = ScalarFieldFactory.RandomScalarField,
            InitialAverageHeight = 8,
            InitialMaxDeviationOfHeight = 2,

            InitialVelocityFunction = VectorFieldFactory.ConstantVectorField,
            InitialAverageVelocity = Vector.Build.DenseOfArray(new[] { 0.0, 0.0, 0.0 }),
            InitialMaxDeviationOfVelocity = 0,

            ColorMapHistoryLength = 1000,
            ColorMapMaterialName = "Materials/Surface",

            ParticleCount = 10000,
            ParticleSpeedScaleFactor = 10,
            ParticleLifespan = 1000,
            ParticleTrailLifespan = 10,
            ParticleMaterialName = "Materials/ParticleMap",

            RadialSpeedSensitivity = 0.1f,

            RotateKey = KeyCode.Mouse0,
            ZoomKey = KeyCode.Mouse1,

            RaiseSurfaceToolKey = KeyCode.UpArrow,
            LowerSurfaceToolKey = KeyCode.DownArrow,
            IncreaseManipulatorMagnitudeKey = KeyCode.RightBracket,
            DecreaseManipulatorMagnitudeKey = KeyCode.LeftBracket,
            ReduceManipulatorRadiusKey = KeyCode.LeftArrow,
            IncreaseManipulatorRadiusKey = KeyCode.RightArrow,

            PauseSimulationKey = KeyCode.P,
            ResetSimulationKey = KeyCode.R,
            
            OptionsMenuKey = KeyCode.O,

            HelpMenuKey = KeyCode.H
        };

        /// <summary>
        /// Returns a default GameOptions object.
        /// </summary>
        /// <returns></returns>
        public static GameOptions Build()
        {
            return InitialOptions;
        }
    }
}
