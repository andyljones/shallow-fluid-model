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
        private readonly GameObject _gameObject;
        private readonly ColorMap _colorMap;

        public PolyhedronRenderer(IPolyhedron polyhedron, Mesh mesh, IDisplayOptions options)
        { 
            _gameObject = CreatePolyhedronObject(mesh, options);
            _colorMap = new ColorMap(mesh, polyhedron);
        }

        private static GameObject CreatePolyhedronObject(Mesh mesh, IDisplayOptions options)
        {
            var polyhedronGameObject = new GameObject("Polyhedron");
            
            var meshFilter = polyhedronGameObject.AddComponent<MeshFilter>();
            meshFilter.mesh = mesh;

            var meshRenderer = polyhedronGameObject.AddComponent<MeshRenderer>();
            meshRenderer.materials = new[]
            {
                Resources.Load<Material>(options.SurfaceMaterialName),
                Resources.Load<Material>(options.WireframeMaterialName)
            };

            return polyhedronGameObject;
        }

        public void Update(PrognosticFields fields)
        {
            _colorMap.Update(fields.Height);
        }
    }
}
