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
        
        private VelocityFieldFactory velocityFieldFactory;
        private FieldUpdater updater;

        private PrognosticFields<Face> fields;
        private PrognosticFields<Face> oldFields;
        private PrognosticFields<Face> olderFields; 

        // Use this for initialization
        void Start ()
        {
            var options = new Options {MinimumNumberOfFaces = 30, Radius = 6000};
            polyhedron = GeodesicSphereFactory.Build(options);
            new PolyhedronRenderer(polyhedron, "Surface", "Materials/Wireframe", "Materials/Surface");

            velocityFieldFactory = new VelocityFieldFactory(polyhedron);
            fieldRenderer = new VectorFieldRenderer(polyhedron, "vectors", "Materials/Vectors");

            fields = new InitialConditions(polyhedron).Fields;

            var parameters = new SimulationParameters
            {
                RotationPeriod = 24*60*60,
                Gravity = 9.8/1000,
                NumberOfRelaxationIterations = 2*options.MinimumNumberOfFaces,
                Timestep = 300
            };

            updater = new FieldUpdater(polyhedron, parameters);

            var velocityField = velocityFieldFactory.VelocityField(fields.Streamfunction, fields.VelocityPotential);

            fieldRenderer.Update(velocityField);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                olderFields = oldFields;
                oldFields = fields;
                fields = updater.Update(fields, oldFields, olderFields);

                var velocityField = velocityFieldFactory.VelocityField(fields.Streamfunction, fields.VelocityPotential);

                fieldRenderer.Update(velocityField);
            }
        }
    }
}
