using System;

namespace Assets.Views.Surface
{
    public interface IPolyhedronRendererOptions
    {
        String SurfaceMaterialName { get; }
        String WireframeMaterialName { get; }
    }
}
