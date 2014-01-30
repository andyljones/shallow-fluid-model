using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Polyhedra;
using MathNet.Numerics.LinearAlgebra;
using UnityEngine;

namespace Assets.Rendering
{
    public static class GraphicsUtilities
    {
        /// <summary>
        /// Converts a MathNet.Numerics vector to a Unity Vector3.
        /// </summary>
        public static Vector3 Vector3(Vector v)
        {
            var x =  (float) v[0];
            var y = -(float) v[1];
            var z =  (float) v[2];

            return new Vector3(x, y, z);
        }

        /// <summary>
        /// Create a game object with a MeshFilter, MeshRenderer and the specified vertices.
        /// </summary>
        public static GameObject CreateRenderingObject(String name, Vector3[] vertices)
        {
            var gameObject = new GameObject(name, new[] { typeof(MeshFilter), typeof(MeshRenderer) });
            var mesh = gameObject.GetComponent<MeshFilter>().mesh;
            mesh.vertices = vertices;
            mesh.uv = Enumerable.Repeat(new Vector2(), vertices.Count()).ToArray();
            mesh.subMeshCount = 2;

            return gameObject;
        }

        /// <summary>
        /// Adds a wireframe to an object.
        /// </summary>
        public static void AddWireframe(GameObject gameObject, int[] lines, String materialName)
        {
            var mesh = gameObject.GetComponent<MeshFilter>().mesh;
            mesh.SetIndices(lines, MeshTopology.Lines, mesh.subMeshCount-1);
            mesh.subMeshCount++;
            //mesh.RecalculateNormals();

            var renderer = gameObject.GetComponent<MeshRenderer>();
            var materials = renderer.materials.ToList();
            materials.Add(Resources.Load(materialName) as Material);
            renderer.materials = materials.ToArray();
        }

        /// <summary>
        /// Adds a surface to an object.
        /// </summary>
        public static void AddSurface(GameObject gameObject, int[] triangles, String materialName)
        {
            var mesh = gameObject.GetComponent<MeshFilter>().mesh;
            mesh.SetIndices(triangles, MeshTopology.Triangles, mesh.subMeshCount-1);
            mesh.subMeshCount++;
            mesh.normals = mesh.vertices.Select(v => v.normalized).ToArray();
            mesh.colors = Enumerable.Repeat(Color.red, mesh.vertexCount).ToArray();

            var renderer = gameObject.GetComponent<MeshRenderer>();
            var materials = renderer.materials.ToList();
            materials.Add(Resources.Load(materialName) as Material);
            renderer.materials = materials.ToArray();
        }

        /// <summary>
        /// Create a dictionary that associates every item in a collection with an index.
        /// </summary>
        public static Dictionary<T, int> CreateItemToIndexMap<T>(IEnumerable<T> vertices)
        {
            var vertexList = vertices.ToList();
            var indices = Enumerable.Range(0, vertexList.Count);
            var vertexIndices = indices.ToDictionary(i => vertexList[i], i => i);

            return vertexIndices;
        }
    }
}
