using System;

namespace Assets.Views.Level.ColorMap
{
    public interface IColorMapOptions
    {
        String ColorMapMaterialName { get; }
        int ColorMapHistoryLength { get; }
    }
}
