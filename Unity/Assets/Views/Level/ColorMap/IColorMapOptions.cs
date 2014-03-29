using System;

namespace Assets.Views.Level.ColorMap
{
    /// <summary>
    /// Options required by the ColorMapView class.
    /// </summary>
    public interface IColorMapOptions
    {
        String ColorMapMaterialName { get; }
        int ColorMapHistoryLength { get; }
    }
}
