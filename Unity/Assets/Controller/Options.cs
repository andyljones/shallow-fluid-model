using System;
using Assets.Views.ColorMap;
using Assets.Views.ParticleMap;
using Engine.Geometry;
using Engine.Simulation;
using MathNet.Numerics.LinearAlgebra;

namespace Assets.Controller
{
    public class Options : IPolyhedronOptions, IColorMapOptions, ISimulationOptions, IParticleMapOptions
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
    }
}
