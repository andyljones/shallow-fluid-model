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
        private VectorFieldRenderer fieldRenderer;
        private IPolyhedron polyhedron;

        // Use this for initialization
        void Start ()
        {
            var options = new Options {MinimumNumberOfFaces = 10000, Radius = 1};
            polyhedron = GeodesicSphereFactory.Build(options);
            new PolyhedronRenderer(polyhedron, "Surface", "Materials/Wireframe", "Materials/Surface");
            Debug.Log(polyhedron.Vertices.Count);
            Debug.Log(polyhedron.Edges.Count);
            Debug.Log(polyhedron.Faces.Count);

            
            fieldRenderer = new VectorFieldRenderer(polyhedron, "vectors", "Materials/Vectors");
        }

        void Update()
        {
            var x = Math.Sin(Time.time);
            var y = Math.Cos(Time.time);
            var z = 0;
            var v = 0.05*new Vector(new [] {x, y, z});
            var values = Enumerable.Repeat(v, polyhedron.Faces.Count).ToArray();
            var field = new VectorField<Face>(polyhedron.IndexOf, values);
    
            fieldRenderer.Update(field);
        }
    }
}
