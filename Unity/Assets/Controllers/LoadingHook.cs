using Engine.Simulation.Initialization;
using MathNet.Numerics.LinearAlgebra;
using UnityEngine;

namespace Assets.Controllers
{
    public class LoadingHook : MonoBehaviour
    {
        private readonly Options _options = new Options
        {
            MinimumNumberOfFaces = 500,
            Radius = 6000,

            Gravity = 10.0 / 1000.0,
            RotationFrequency = 1.0 / (3600.0 * 24.0),
            Timestep = 200,

            InitialHeightFunction = ScalarFieldFactory.RandomScalarField,
            InitialAverageHeight = 8,
            InitialMaxDeviationOfHeight = 0.1,

            InitialVelocityFunction = VectorFieldFactory.ConstantVectorField,
            InitialAverageVelocity = new Vector(new[] { 0.0, 0.0, 0.0}),
            InitialMaxDeviationOfVelocity = 0,

            ColorMapHistoryLength = 1000,
            ColorMapMaterialName = "Materials/Surface",

            ParticleCount = 20000,
            ParticleSpeedScaleFactor = 10,
            ParticleLifespan = 1000,
            ParticleTrailLifespan = 10,
            ParticleMaterialName = "Materials/ParticleMap",
        };

        private MainController _main;

        void Start ()
        {
            _main = new MainController(_options);
        }

        void Update()
        {
            _main.Update();
        }

        void OnGUI()
        {
            _main.UpdateGUI();
        }

        void OnApplicationQuit()
        {
            _main.Terminate();
        }
    }
}
