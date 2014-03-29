using System;
using Engine.Geometry;
using Engine.Simulation;
using UnityEngine;

namespace Assets.Views.Level.ColorMap
{
    /// <summary>
    /// Displays a colormap of a scalar field..
    /// </summary>
    public class ColorMapView : IDisposable
    {
        private readonly FieldColorer _fieldColorer;
        private readonly GameObjectManager _gameObjectManager;

        //TODO: Surface & Mesh should be supplied as a single object.
        /// <summary>
        /// Construct a ColorMapView which will display colormaps defined over the given IPolyhedron on the given mesh.
        /// </summary>
        /// <param name="surface"></param>
        /// <param name="mesh"></param>
        /// <param name="options"></param>
        public ColorMapView(IPolyhedron surface, Mesh mesh, IColorMapOptions options)
        {
            _fieldColorer = new FieldColorer(surface, options.ColorMapHistoryLength);
         
            _gameObjectManager = new GameObjectManager(mesh, options.ColorMapMaterialName);
        }

        /// <summary>
        /// Servant for Unity's Update() function. Updates the colormap with the specified field.
        /// </summary>
        /// <param name="field"></param>
        public void Update(ScalarField<Face> field)
        {
            var colors = _fieldColorer.Color(field);
            _gameObjectManager.Set(colors);
        }

        #region Destruction methods
        /// <summary>
        /// Destroys the game object which the colormap is displayed on.
        /// </summary>
        public void Dispose()
        {
            _gameObjectManager.Dispose();
        }
        #endregion
    }
}
