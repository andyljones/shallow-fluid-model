using System;
using Assets.Controllers.Manipulator;
using Assets.Views.ColorMap;
using Assets.Views.ParticleMap;
using Engine.Geometry;
using Engine.Simulation;
using MathNet.Numerics.LinearAlgebra;
using UnityEngine;

namespace Assets.Controllers
{
    public class Options : IPolyhedronOptions, IColorMapOptions, ISimulationOptions, IParticleMapOptions, IFieldManipulatorOptions
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

        public KeyCode SurfaceRaiseKey { get; set; }
        public KeyCode SurfaceLowerKey { get; set; }
        public KeyCode RadiusIncreaseKey { get; set; }
        public KeyCode RadiusDecreaseKey { get; set; }
        public KeyCode IntensityIncreaseKey { get; set; }
        public KeyCode IntensityDecreaseKey { get; set; }
    }
}
