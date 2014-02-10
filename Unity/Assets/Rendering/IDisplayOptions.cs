using System;
namespace Assets.Rendering
{
    public interface IDisplayOptions
    {
        String SurfaceMaterialName { get; }
        String WireframeMaterialName { get; }
    }
}
