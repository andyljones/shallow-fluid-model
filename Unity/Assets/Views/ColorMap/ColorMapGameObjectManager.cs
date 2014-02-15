using Assets.Controller.Manipulator;
using Engine.Geometry;
using Engine.Simulation;
using UnityEngine;

namespace Assets.Views.ColorMap
{
    public class ColorMapGameObjectManager
    {
        private readonly MeshFilter _meshFilter;

        public ColorMapGameObjectManager(IPolyhedron polyhedron, IColorMapOptions options)
        {
            var gameObject = BuildGameObject(polyhedron, options.SurfaceMaterialName);
            _meshFilter = gameObject.GetComponent<MeshFilter>();
        }

        private static GameObject BuildGameObject(IPolyhedron polyhedron, string surfaceMaterialName)
        {
            var gameObject = new GameObject("Polyhedron");

            var meshFilter = gameObject.AddComponent<MeshFilter>();
            meshFilter.mesh = MeshFactory.Build(polyhedron);

            var meshRenderer = gameObject.AddComponent<MeshRenderer>();
            meshRenderer.materials = new[]
            {
                Resources.Load<Material>(surfaceMaterialName)
            };

            return gameObject;
        }

        public void Set(Color[] colors)
        {
            _meshFilter.mesh.colors = colors;
        }
    }
}
