using System.Collections.Generic;
using System.Linq;
using Engine.Geometry;
using Engine.Simulation;
using UnityEngine;

namespace Assets.Views.ColorMap
{
    public class ColorMapView
    {
        private readonly FieldColorer _fieldColorer;
        private readonly ColorMapGameObjectManager _gameObjectManager;

        public ColorMapView(IPolyhedron polyhedron, IColorMapOptions options)
        {
            _fieldColorer = new FieldColorer(polyhedron);
            _gameObjectManager = new ColorMapGameObjectManager(polyhedron, options);
        }

        public void Update(ScalarField<Face> field)
        {
            var colors = _fieldColorer.Color(field);
            _gameObjectManager.Set(colors);
        }
    }
}
