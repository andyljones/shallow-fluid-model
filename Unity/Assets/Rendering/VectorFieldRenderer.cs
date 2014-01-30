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
        private Vector3[] _origins;
        private TangentSpace[] _tangentSpaces;
        private GameObject _gameObject;

        public VectorFieldRenderer(IPolyhedron polyhedron, String name, String materialName)
        {
            _origins = FindOrigins(polyhedron);
            _tangentSpaces = ConstructTangentSpaces(polyhedron);

            _gameObject = CreateRenderingObject(name, materialName, polyhedron.Faces.Count);

        }

        private static GameObject CreateRenderingObject(String name, String materialName, int numberOfFaces)
        {
            var gameObject = new GameObject(name, new[] { typeof(MeshFilter), typeof(MeshRenderer) });

            var mesh = gameObject.GetComponent<MeshFilter>().mesh;
            mesh.vertices = new Vector3[2*numberOfFaces];
            mesh.uv = Enumerable.Repeat(new Vector2(), mesh.vertexCount).ToArray();
            mesh.normals = mesh.vertices.Select(v => v.normalized).ToArray();
            mesh.SetIndices(Enumerable.Range(0, mesh.vertexCount).ToArray(), MeshTopology.Lines, 0);

            var renderer = gameObject.GetComponent<MeshRenderer>();
            renderer.material = Resources.Load(materialName, typeof(Material)) as Material;

            return gameObject;
        }

        private static Vector3[] FindOrigins(IPolyhedron polyhedron)
        {
            var origins = new Vector3[polyhedron.Faces.Count];
            foreach (var face in polyhedron.Faces)
            {
                var origin = GraphicsUtilities.Vector3(face.SphericalCenter());
                origins[polyhedron.IndexOf(face)] = origin;
            }

            return origins;
        }

        private static TangentSpace[] ConstructTangentSpaces(IPolyhedron polyhedron)
        {
            var globalNorth = VectorUtilities.NewVector(0, 0);

            var tangentSpaces = new TangentSpace[polyhedron.Faces.Count];
            foreach (var face in polyhedron.Faces)
            {
                var up = face.SphericalCenter().Normalize();
                var east = Vector.CrossProduct(globalNorth, up).Normalize();
                var north = Vector.CrossProduct(up, east).Normalize();
                tangentSpaces[polyhedron.IndexOf(face)] = new TangentSpace(east, north, up);
            }

            return tangentSpaces;
        }

        public void Update(VectorField<Face> field)
        {
            var newVertices = new Vector3[2*field.Count];
            for (int i = 0; i < field.Count; i++)
            {
                newVertices[2*i] = _origins[i];
                newVertices[2*i + 1] = _origins[i] + (float)(1500/.2)* GraphicsUtilities.Vector3(field[i]);
            }

            _gameObject.GetComponent<MeshFilter>().mesh.vertices = newVertices;
        }
    }
}
