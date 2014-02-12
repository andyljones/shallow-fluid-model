using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Rendering.WindMapper
{
    public interface IWindMapOptions
    {
        double Radius { get; }
        double Timestep { get; }
        double WindmapScaleFactor { get; }
        int ParticleCount { get; }
        double RenewalRate { get; }

        String WindMapMaterialName { get; }
    }
}
