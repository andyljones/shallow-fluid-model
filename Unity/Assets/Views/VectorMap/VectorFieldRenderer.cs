using System;
using System.Linq;
using Assets.Views.Surface;
using Engine.Models;
using Engine.Polyhedra;
using UnityEngine;

namespace Assets.Views.VectorMap
{
    public class VectorFieldRenderer
    {
        private readonly Vector3[] _origins;
        private readonly GameObject _gameObject;

        private readonly IPolyhedron _polyhedron;

        public VectorFieldRenderer(IPolyhedron polyhedron, String name, String materialName)
        {
            _polyhedron = polyhedron;
            _origins = FindOrigins(polyhedron);

            _gameObject = CreateRenderingObject(name, materialName, polyhedron.Vertices.Count);

        }

        private static GameObject CreateRenderingObject(String name, String materialName, int numberOfVectors)
        {
            var gameObject = new GameObject(name, new[] { typeof(MeshFilter), typeof(MeshRenderer) });

            var mesh = gameObject.GetComponent<MeshFilter>().mesh;
            mesh.vertices = new Vector3[2*numberOfVectors];
            mesh.uv = Enumerable.Repeat(new Vector2(), mesh.vertexCount).ToArray();
            mesh.normals = mesh.vertices.Select(v => v.normalized).ToArray();
            mesh.SetIndices(Enumerable.Range(0, mesh.vertexCount).ToArray(), MeshTopology.Lines, 0);

            var renderer = gameObject.GetComponent<MeshRenderer>();
            renderer.material = Resources.Load(materialName, typeof(Material)) as Material;

            return gameObject;
        }

        private static Vector3[] FindOrigins(IPolyhedron polyhedron)
        {
            var vertexVectors = polyhedron.Vertices.Select(vertex => GraphicsUtilities.Vector3(vertex.Position));

            return vertexVectors.ToArray();
        }

        public void Update(VectorField<Vertex> field)
        {
            var scaleFactor = 250f;
            var max = (float)field.Values.Max(v => v.Norm());

            var numberOfVertices = _polyhedron.Vertices.Count;

            var newVertices = new Vector3[2*numberOfVertices];
            for (int i = 0; i < numberOfVertices; i++)
            {
                newVertices[2*i + 1] = 1.01f*_origins[i];
                newVertices[2*i] = 1.01f*_origins[i] + (scaleFactor/max)*GraphicsUtilities.Vector3(field[i]);
            }


            _gameObject.GetComponent<MeshFilter>().mesh.vertices = newVertices;
        }
    }
}
