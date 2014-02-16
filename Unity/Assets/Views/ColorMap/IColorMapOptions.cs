using System;

namespace Assets.Views.ColorMap
{
    public interface IColorMapOptions
    {
        String ColorMapMaterialName { get; }
        int ColorMapHistoryLength { get; }
    }
}
