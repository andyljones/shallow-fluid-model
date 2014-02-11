using System;
namespace Assets.Rendering
{
    public interface IPolyhedronRendererOptions
    {
        String SurfaceMaterialName { get; }
        String WireframeMaterialName { get; }
    }
}
