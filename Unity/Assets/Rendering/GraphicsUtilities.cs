using System;
using System.Linq;
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

        ///// <summary>
        ///// Create a game object with a MeshFilter, MeshRenderer and the specified vertices.
        ///// </summary>
        //public static GameObject CreateRenderingObject(String name, Vector3[] vertices)
        //{
        //    var gameObject = new GameObject(name, new[] { typeof(MeshFilter), typeof(MeshRenderer) });
        //    var mesh = gameObject.GetComponent<MeshFilter>().mesh;
        //    mesh.vertices = vertices;
        //    mesh.uv = Enumerable.Repeat(new Vector2(), vertices.Count()).ToArray();
        //    mesh.subMeshCount = 2;

        //    return gameObject;
        //}

        ///// <summary>
        ///// Adds a wireframe to an object.
        ///// </summary>
        //public static void AddWireframe(GameObject gameObject, int[] lines, String materialName)
        //{
        //    var mesh = gameObject.GetComponent<MeshFilter>().mesh;
        //    mesh.SetIndices(lines, MeshTopology.Lines, mesh.subMeshCount-1);
        //    mesh.subMeshCount++;
        //    //mesh.RecalculateNormals();

        //    var renderer = gameObject.GetComponent<MeshRenderer>();
        //    var materials = renderer.materials.ToList();
        //    materials.Add(Resources.Load(materialName) as Material);
        //    renderer.materials = materials.ToArray();
        //}

        ///// <summary>
        ///// Adds a surface to an object.
        ///// </summary>
        //public static void AddSurface(GameObject gameObject, int[] triangles, String materialName)
        //{
        //    var mesh = gameObject.GetComponent<MeshFilter>().mesh;
        //    mesh.SetIndices(triangles, MeshTopology.Triangles, mesh.subMeshCount-1);
        //    mesh.subMeshCount++;
        //    mesh.normals = mesh.vertices.Select(v => v.normalized).ToArray();

        //    var renderer = gameObject.GetComponent<MeshRenderer>();
        //    var materials = renderer.materials.ToList();
        //    materials.Add(Resources.Load(materialName) as Material);
        //    renderer.materials = materials.ToArray();
        //}
    }
}
