using System;
using Engine.Geometry;
using Engine.Simulation;

namespace Assets.Views.ColorMap
{
    public class ColorMapView
    {
        public readonly Func<int, Face> FaceAtTriangleIndex;

        private readonly FieldColorer _fieldColorer;
        private readonly ColorMapGameObjectManager _gameObjectManager;

        public ColorMapView(IPolyhedron surface, IColorMapOptions options)
        {
            _fieldColorer = new FieldColorer(surface, options.ColorMapHistoryLength);
         
            var meshManager = new MeshManager(surface);
            FaceAtTriangleIndex = meshManager.FaceAtTriangleIndex;
            _gameObjectManager = new ColorMapGameObjectManager(meshManager.Mesh, options.ColorMapMaterialName);
        }

        public void Update(ScalarField<Face> field)
        {
            var colors = _fieldColorer.Color(field);
            _gameObjectManager.Set(colors);
        }
    }
}
