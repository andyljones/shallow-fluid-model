using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Rendering;
using Engine;
using Engine.Models;
using Engine.Models.MomentumModel;
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
        private PrognosticFieldsUpdater updater;

        private PrognosticFields fields;
        private PrognosticFields oldFields;
        private PrognosticFields olderFields;

        private PrognosticFieldsFactory fieldsFactory;

        private SimulationParameters parameters;

        private VectorFieldOperators _vOperators;
        private VectorField<Vertex> _constantVectorField;

        // Use this for initialization
        void Start ()
        {
            var options = new Options {MinimumNumberOfFaces = 400, Radius = 6000};
            polyhedron = GeodesicSphereFactory.Build(options);
            polyhedronRenderer = new PolyhedronRenderer(polyhedron, "Surface", "Materials/Wireframe", "Materials/Surface");

            fieldRenderer = new VectorFieldRenderer(polyhedron, "vectors", "Materials/Vectors");


            fieldsFactory = new PrognosticFieldsFactory(polyhedron);
            fieldsFactory.Height = fieldsFactory.RandomScalarField(10, 1);
            _constantVectorField = fieldsFactory.ConstantVectorField(0, 0);            
            fieldsFactory.Velocity = _constantVectorField;
            fields = fieldsFactory.Build();


            parameters = new SimulationParameters
            {
                RotationFrequency = 1.0/(24*3600),
                Gravity = 10.0 / 1000.0,
                Timestep = 400
            };

            updater = new PrognosticFieldsUpdater(polyhedron, parameters);

            fieldRenderer.Update(fields.Velocity);

            _vOperators = new VectorFieldOperators(polyhedron);
            //Debug.Log(_vOperators.Gradient(fields.Height));

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
                    var seconds = (int)(counter * parameters.Timestep);
                    var days = seconds / (3600 * 24);
                    var hours = seconds / 3600 - 24 * days;
                    Debug.Log(String.Format("{0}D {1}H", days, hours));

                    //fields.Velocity = _constantVectorField;

                    olderFields = oldFields;
                    oldFields = fields;
                    fields = updater.Update(fields, oldFields, olderFields);
                    //Debug.Log(_vOperators.FluxDivergence(fields.Velocity, fields.Height).Values.Sum());

                    fieldRenderer.Update(fields.Velocity);
                    polyhedronRenderer.Update(fields.Height);
                }
            }
        }
    }
}
