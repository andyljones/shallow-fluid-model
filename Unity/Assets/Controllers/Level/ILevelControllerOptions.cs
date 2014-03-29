using Assets.Controllers.Level.GameCamera;
using Assets.Controllers.Level.Manipulator;
using Assets.Controllers.Level.Simulation;
using Assets.Views.Level.ColorMap;
using Assets.Views.Level.ParticleMap;
using Engine.Geometry;

namespace Assets.Controllers.Level
{
    /// <summary>
    /// Specifies the options needed to construct the LevelController object.
    /// </summary>
    public interface ILevelControllerOptions : IPolyhedronOptions, IColorMapOptions, ISimulationControllerOptions, IParticleMapOptions, IFieldManipulatorOptions, ICameraOptions
    {
        // Redeclarations necessary to suppress ambiguous reference error.
        new double Radius { get; }
        new double Timestep { get; }
    }
}
