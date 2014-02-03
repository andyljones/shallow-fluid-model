using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Rendering;
using Engine;
using Engine.Models;
using Engine.Models.VorticityDivergenceModel;
using Engine.Polyhedra;
using Engine.Polyhedra.IcosahedronBased;
using MathNet.Numerics.LinearAlgebra;
using UnityEngine;

namespace Assets
{
    public class TestHook : MonoBehaviour
    {
        private bool freerunning = false;

        private int counter = 0;

        private VectorFieldRenderer fieldRenderer;
        private IPolyhedron polyhedron;

        private PolyhedronRenderer polyhedronRenderer;
        private VelocityFieldFactory velocityFieldFactory;
        private PrognosticFieldsUpdater updater;

        private PrognosticFields<Face> fields;
        private PrognosticFields<Face> oldFields;
        private PrognosticFields<Face> olderFields;

        private ScalarFieldOperators _operators;
        private FieldIntegrator _integrator;
        private PrognosticFieldsFactory fieldsFactory;

        private SimulationParameters parameters;

        // Use this for initialization
        void Start ()
        {
            var options = new Options {MinimumNumberOfFaces = 200, Radius = 6000};
            polyhedron = GeodesicSphereFactory.Build(options);
            polyhedronRenderer = new PolyhedronRenderer(polyhedron, "Surface", "Materials/Wireframe", "Materials/Surface");

            velocityFieldFactory = new VelocityFieldFactory(polyhedron);
            fieldRenderer = new VectorFieldRenderer(polyhedron, "vectors", "Materials/Vectors");

            fieldsFactory = new PrognosticFieldsFactory(polyhedron);
            fieldsFactory.Height = fieldsFactory.XDependentField(10, .1);
            fieldsFactory.AbsoluteVorticity = fieldsFactory.XDependentField(0, 0.000005);
            fields = fieldsFactory.Build();

            parameters = new SimulationParameters
            {
                RotationFrequency = 1.0/(24.0*3600.0)*0.3,
                Gravity = 10.0/1000.0,
                NumberOfRelaxationIterations = polyhedron.Faces.Count/4,
                Timestep = 300
            };

            updater = new PrognosticFieldsUpdater(polyhedron, parameters);

            var velocityField = velocityFieldFactory.VelocityField(fields.Streamfunction, fields.VelocityPotential);

            fieldRenderer.Update(velocityField);

            _operators = new ScalarFieldOperators(polyhedron);
            _integrator = new FieldIntegrator(polyhedron, parameters);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                freerunning = !freerunning;
            }

            if (Input.GetKeyDown(KeyCode.F) || Input.GetKey(KeyCode.N) || freerunning)
            {
                for (int i = 0; i < 1; i++)
                {
                    counter++;
                    var seconds = (int)(counter*parameters.Timestep);
                    var days = seconds/(3600*24);
                    var hours = seconds/3600 - 24*days;
                    Debug.Log(String.Format("{0}D {1}H", days, hours));
                    olderFields = oldFields;
                    oldFields = fields;
                    fields = updater.Update(fields, oldFields, olderFields);

                    var velocityField = velocityFieldFactory.VelocityField(fields.Streamfunction, fields.VelocityPotential);

                    fieldRenderer.Update(velocityField);
                    polyhedronRenderer.Update(fields.Height);
                }
            }
        }
    }
}
