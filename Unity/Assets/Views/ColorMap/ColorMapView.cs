using System;
using Engine.Geometry;
using Engine.Simulation;
using UnityEngine;

namespace Assets.Views.ColorMap
{
    public class ColorMapView
    {
        private readonly FieldColorer _fieldColorer;
        private readonly ColorMapGameObjectManager _gameObjectManager;

        public ColorMapView(IPolyhedron surface, Mesh mesh, IColorMapOptions options)
        {
            _fieldColorer = new FieldColorer(surface, options.ColorMapHistoryLength);
         
            _gameObjectManager = new ColorMapGameObjectManager(mesh, options.ColorMapMaterialName);
        }

        public void Update(ScalarField<Face> field)
        {
            var colors = _fieldColorer.Color(field);
            _gameObjectManager.Set(colors);
        }
    }
}
