using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Polyhedra;
using UnityEngine;

namespace Assets.Rendering
{
    /// <summary>
    /// Renders a Polyhedron as a solid object with the requested properties.
    /// </summary>
    public class SolidPolyhedronRenderer
    {
        private readonly GameObject gameObject;
        private readonly Dictionary<Vertex, int> vertexIndices; 

        public SolidPolyhedronRenderer(Polyhedron polyhedron, String name, String materialName)
        {
            vertexIndices = InitializeVertexToIndexMap(polyhedron.Vertices);

            var mesh = CreateMesh(polyhedron, vertexIndices); 
            gameObject = CreateRenderingObject(name, materialName, mesh);
        }

        private static Dictionary<Vertex, int> InitializeVertexToIndexMap(IEnumerable<Vertex> vertices)
        {
            var vertexList = vertices.ToList();
            var indices = Enumerable.Range(0, vertexList.Count);
            var vertexIndices = indices.ToDictionary(i => vertexList[i], i => i);

            return vertexIndices;
        }

        #region CreateMesh methods
        private static Mesh CreateMesh(Polyhedron polyhedron, Dictionary<Vertex, int> vertexIndices)
        {
            var mesh = new Mesh
            {
                vertices = CreateVertexArray(vertexIndices),
                triangles = CreateTriangleArray(vertexIndices, polyhedron.Faces),
                uv = new Vector2[] { }
            };
            mesh.RecalculateNormals();

            return mesh;
        }

        private static Vector3[] CreateVertexArray(Dictionary<Vertex, int> vertexIndices)
        {
            var unityVertices = new Vector3[vertexIndices.Count];
            foreach (var vertexAndIndex in vertexIndices)
            {
                var vertex = vertexAndIndex.Key;
                var index = vertexAndIndex.Value;

                unityVertices[index] = GraphicsUtilities.Vector3(vertex.Position);
            }

            return unityVertices;
        }

        private static int[] CreateTriangleArray(Dictionary<Vertex, int> vertexIndices, IEnumerable<Face> faces)
        {
            return faces.SelectMany(face => Indices(vertexIndices, face)).ToArray();
        }

        private static IEnumerable<int> Indices(Dictionary<Vertex, int> vertexIndices, Face face)
        {
            return face.Vertices.Select(vertex => vertexIndices[vertex]);
        }
        #endregion

        private static GameObject CreateRenderingObject(String name, String materialName, Mesh mesh)
        {
            var gameObject = new GameObject(name, new []{typeof(MeshFilter), typeof(MeshRenderer)});
            var material = Resources.Load(materialName, typeof(Material)) as Material;

            Debug.Log(material);

            gameObject.GetComponent<MeshRenderer>().material = material;
            gameObject.GetComponent<MeshFilter>().mesh = mesh;
            
            return gameObject;
        }
    }
}
