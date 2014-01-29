using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Rendering;
using Engine;
using Engine.Polyhedra;
using Engine.Polyhedra.IcosahedronBased;
using Engine.Simulation;
using MathNet.Numerics.LinearAlgebra;
using UnityEngine;

namespace Assets
{
    public class TestHook : MonoBehaviour
    {
        private Dictionary<Face, int> index; 
        private VectorFieldRenderer renderer;

        // Use this for initialization
        void Start ()
        {
            var options = new Options {MinimumNumberOfFaces = 1000, Radius = 1};
            var polyhedron = GeodesicSphereFactory.Build(options);
            new PolyhedronRenderer(polyhedron, "Surface", "Materials/Wireframe", "Materials/Surface");
            Debug.Log(polyhedron.Vertices.Count);
            Debug.Log(polyhedron.Edges.Count);
            Debug.Log(polyhedron.Faces.Count);

            index = new Dictionary<Face, int>();
            for (int i = 0; i < polyhedron.Faces.Count; i++)
            {
                index.Add(polyhedron.Faces[i], i);
            }
            
            renderer = new VectorFieldRenderer(index, polyhedron, "vectors", "Materials/Vectors");
        }

        void Update()
        {
            var x = Math.Sin(Time.time);
            var y = Math.Cos(Time.time);
            var z = 0;
            var v = 0.1*new Vector(new [] {x, y, z});
            var values = Enumerable.Repeat(v, index.Count).ToArray();
            var field = new VectorField<Face>(index, values);
    
            renderer.Update(field);
        }
    }
}
