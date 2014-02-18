﻿using Assets.Controllers.Level.Manipulator;
using Assets.Controllers.Level.Simulation;
using Assets.Views.ColorMap;
using Assets.Views.ParticleMap;
using Engine.Geometry;
using UnityEngine;

namespace Assets.Controllers.Level
{
    public interface ILevelControllerOptions : IPolyhedronOptions, IColorMapOptions, ISimulationControllerOptions, IParticleMapOptions, IFieldManipulatorOptions
    {
        // Redeclarations necessary to suppress ambiguous reference error.
        new double Radius { get; }
        new double Timestep { get; }

        KeyCode ResetKey { get; }
    }
}