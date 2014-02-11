using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Models;
using Engine.Models.MomentumModel;
using Engine.Polyhedra;
using UnityEngine;

namespace Assets.Rendering
{
    /// <summary>
    /// Renders a Polyhedron as a solid object with the requested properties.
    /// </summary>
    public class PolyhedronRenderer
    {
        private readonly GameObject _gameGameObject;
        private readonly ColorMap _colorMap;

        private MeshFilter _meshFilter;
        private readonly Mesh _mesh;

        public PolyhedronRenderer(GameObject polyhedronGameObject, IPolyhedron polyhedron, Mesh mesh, IPolyhedronRendererOptions options)
        {
            _gameGameObject = polyhedronGameObject;
            _mesh = mesh;
            _meshFilter = InitializePolyhedronRenderer(polyhedronGameObject, mesh, options);
            _colorMap = new ColorMap(mesh, polyhedron);
        }

        private static MeshFilter InitializePolyhedronRenderer(GameObject polyhedronGameObject, Mesh mesh, IPolyhedronRendererOptions options)
        {
            var meshFilter = polyhedronGameObject.AddComponent<MeshFilter>();
            meshFilter.mesh = mesh;

            var meshRenderer = polyhedronGameObject.AddComponent<MeshRenderer>();
            meshRenderer.materials = new[]
            {
                Resources.Load<Material>(options.SurfaceMaterialName)
                //Resources.Load<Material>(options.WireframeMaterialName)
            };

            return meshFilter;
        }

        public void Update(PrognosticFields fields)
        {
            _colorMap.Update(fields.Height);
        }
    }
}
