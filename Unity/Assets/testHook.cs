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

        private FieldOperators _operators;

        // Use this for initialization
        void Start ()
        {
            var options = new Options {MinimumNumberOfFaces = 100, Radius = 6000};
            polyhedron = GeodesicSphereFactory.Build(options);
            new PolyhedronRenderer(polyhedron, "Surface", "Materials/Wireframe", "Materials/Surface");

            velocityFieldFactory = new VelocityFieldFactory(polyhedron);
            fieldRenderer = new VectorFieldRenderer(polyhedron, "vectors", "Materials/Vectors");

            var fieldsFactory = new PrognosticFieldsFactory(polyhedron);
            fieldsFactory.Height = fieldsFactory.SinusoidalField(8, 1);
            fields = fieldsFactory.Build();

            var parameters = new SimulationParameters
            {
                RotationFrequency = 0,
                Gravity = 10.0/1000.0,
                NumberOfRelaxationIterations = 200,
                Timestep = 300
            };

            updater = new FieldUpdater(polyhedron, parameters);

            var velocityField = velocityFieldFactory.VelocityField(fields.Streamfunction, fields.VelocityPotential);

            fieldRenderer.Update(velocityField);

            _operators = new FieldOperators(polyhedron);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.F) || Input.GetKey(KeyCode.N))
            {
                for (int i = 0; i < 1; i++)
                {
                    //Debug.Log(fields.ToString(5));
                    //Debug.Log(fields.Height);
                    olderFields = oldFields;
                    oldFields = fields;
                    fields = updater.Update(fields, oldFields, olderFields);
                    var velocityField = velocityFieldFactory.VelocityField(fields.Streamfunction, fields.VelocityPotential);
                    //Debug.Log(velocityField.Values.Max(value => value[2]));

                    fieldRenderer.Update(velocityField);
                }
            }
        }
    }
}
