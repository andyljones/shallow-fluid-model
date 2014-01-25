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
        /// Create a game object with MeshFilter/MeshRenderer and the specified name, material and mesh.
        /// </summary>
        public static GameObject CreateRenderingObject(String name, String materialName, Mesh mesh)
        {
            var gameObject = new GameObject(name, new[] { typeof(MeshFilter), typeof(MeshRenderer) });
            var material = Resources.Load(materialName, typeof(Material)) as Material;

            gameObject.GetComponent<MeshRenderer>().materials = new Material[] {material, material};
            gameObject.GetComponent<MeshFilter>().mesh = mesh;

            return gameObject;
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
