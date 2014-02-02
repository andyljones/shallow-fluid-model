using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Polyhedra;
using Engine.Simulation;
using Engine.Utilities;
using MathNet.Numerics.LinearAlgebra;
using UnityEngine;

namespace Assets.Rendering
{
    public class VectorFieldRenderer
    {
        private readonly Vector3[] _origins;
        private TangentSpace[] _tangentSpaces;
        private readonly GameObject _gameObject;

        private readonly IPolyhedron _polyhedron;

        public VectorFieldRenderer(IPolyhedron polyhedron, String name, String materialName)
        {
            _polyhedron = polyhedron;
            _origins = FindOrigins(polyhedron);
            //_tangentSpaces = ConstructTangentSpaces(polyhedron);

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

        //private static TangentSpace[] ConstructTangentSpaces(IPolyhedron polyhedron)
        //{
        //    var globalNorth = VectorUtilities.NewVector(0, 0);

        //    var tangentSpaces = new TangentSpace[polyhedron.Faces.Count];
        //    foreach (var face in polyhedron.Faces)
        //    {
        //        var up = face.SphericalCenter().Normalize();
        //        var east = Vector.CrossProduct(globalNorth, up).Normalize();
        //        var north = Vector.CrossProduct(up, east).Normalize();
        //        tangentSpaces[polyhedron.IndexOf(face)] = new TangentSpace(east, north, up);
        //    }

        //    return tangentSpaces;
        //}

        public void Update(VectorField<Vertex> field)
        {
            var scaleFactor = (float) (1000/.2);
            //var numberOfFaces = _polyhedron.Faces.Count;
            var numberOfVertices = _polyhedron.Vertices.Count;

            //var newVertices = new Vector3[2*(numberOfFaces + numberOfVertices)];
            //for (int i = 0; i < numberOfFaces; i++)
            //{
            //    newVertices[2*i] = _origins[i];
            //    newVertices[2*i + 1] = _origins[i] + scaleFactor * GraphicsUtilities.Vector3(field[i]);
            //}
            var newVertices = new Vector3[2*numberOfVertices];

            for (int i = 0; i < numberOfVertices; i++)
            {
                var average = field[i];
                newVertices[2*i] = _origins[i];
                newVertices[2*i + 1] = _origins[i] + scaleFactor*GraphicsUtilities.Vector3(average);
            }


            _gameObject.GetComponent<MeshFilter>().mesh.vertices = newVertices;
        }
    }
}
