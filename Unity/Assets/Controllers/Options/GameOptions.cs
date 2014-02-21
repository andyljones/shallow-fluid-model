using System;
using Assets.Controllers.Level;
using Assets.Views.Options;
using Assets.Views.UI.Help;
using Engine.Geometry;
using Engine.Simulation;
using MathNet.Numerics.LinearAlgebra;
using UnityEngine;

namespace Assets.Controllers.Options
{
    public class GameOptions : ILevelControllerOptions, IOptionsControllerOptions, IHelpOptions
    {
        public double Radius { get; set; }
        public int MinimumNumberOfFaces { get; set; }
        public double RotationFrequency { get; set; }
        public double Gravity { get; set; }
        public double Timestep { get; set; }

        public Func<IPolyhedron, double, double, ScalarField<Face>> InitialHeightFunction { get;  set; }
        public double InitialAverageHeight { get;  set; }
        public double InitialMaxDeviationOfHeight { get;  set; }
        public Func<IPolyhedron, Vector, double, VectorField<Vertex>> InitialVelocityFunction { get;  set; }
        public Vector InitialAverageVelocity { get;  set; }
        public double InitialMaxDeviationOfVelocity { get;  set; }

        public string ColorMapMaterialName { get; set; }
        public int ColorMapHistoryLength { get; set; }

        public double ParticleSpeedScaleFactor { get; set; }
        public int ParticleCount { get; set; }
        public int ParticleLifespan { get; set; }
        public string ParticleMaterialName { get; set; }
        public int ParticleTrailLifespan { get; set; }

        public float RadialSpeedSensitivity { get; set; }

        public KeyCode RotateKey { get; set; }
        public KeyCode ZoomKey { get; set; }

        public KeyCode RaiseSurfaceToolKey { get; set; }
        public KeyCode LowerSurfaceToolKey { get; set; }
        public KeyCode IncreaseManipulatorRadiusKey { get; set; }
        public KeyCode ReduceManipulatorRadiusKey { get; set; }
        public KeyCode IncreaseManipulatorMagnitudeKey { get; set; }
        public KeyCode DecreaseManipulatorMagnitudeKey { get; set; }

        public KeyCode PauseSimulationKey { get; set; }
        public KeyCode ResetSimulationKey { get; set; }

        public KeyCode OptionsMenuKey { get; set; }

        public KeyCode HelpMenuKey { get; set; }
    }
}
