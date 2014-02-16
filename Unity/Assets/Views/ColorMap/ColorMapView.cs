using System.Collections.Generic;
using System.Linq;
using Engine.Geometry;
using Engine.Simulation;
using UnityEngine;

namespace Assets.Views.ColorMap
{
    public class ColorMapView
    {
        public readonly MeshManager MeshManager;

        private readonly FieldColorer _fieldColorer;
        private readonly ColorMapGameObjectManager _gameObjectManager;

        public ColorMapView(IPolyhedron surface, IColorMapOptions options)
        {
            _fieldColorer = new FieldColorer(surface);
         
            MeshManager = new MeshManager(surface);
            _gameObjectManager = new ColorMapGameObjectManager(MeshManager.Mesh, options);
        }

        public void Update(ScalarField<Face> field)
        {
            var colors = _fieldColorer.Color(field);
            _gameObjectManager.Set(colors);
        }
    }
}
