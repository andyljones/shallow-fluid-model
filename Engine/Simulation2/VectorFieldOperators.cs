using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Polyhedra;
using Engine.Simulation;
using MathNet.Numerics.LinearAlgebra;

namespace Engine.Simulation2
{
    public class VectorFieldOperators
    {
        private IPolyhedron _polyhedron;

        private Vector[][] _edgeNormals;
        private double[][] _bisectorDistances;
        private int[][] _faces;
        private double[] _areas;

        public VectorFieldOperators(IPolyhedron polyhedron)
        {
            _polyhedron = polyhedron;

            _edgeNormals = VertexIndexedTableFactory.EdgeNormals(polyhedron);
            _bisectorDistances = VertexIndexedTableFactory.BisectorDistances(polyhedron);
            _faces = VertexIndexedTableFactory.Faces(polyhedron);
            _areas = VertexIndexedTableFactory.Areas(polyhedron);
        }

        #region Gradient methods
        /// <summary>
        /// The discrete gradient, as described in Randall & Ringler 2001.
        /// </summary>
        public VectorField<Vertex> Gradient(ScalarField<Face> A)
        {
            var numberOfVertices = _polyhedron.Vertices.Count;
            
            var results = new Vector[numberOfVertices];
            foreach (var vertex in Enumerable.Range(0, numberOfVertices))
            {
                results[vertex] = GradientAtVertex(vertex, A);
            }
            return new VectorField<Vertex>(_polyhedron.IndexOf, results);
        }

        private Vector GradientAtVertex(int vertex, ScalarField<Face> A)
        {
            var faces = _faces[vertex];
            var bisectorDistances = _bisectorDistances[vertex];
            var edgeNormals = _edgeNormals[vertex];

            var result = Vector.Zeros(3);
            result += A[faces[0]] * (bisectorDistances[faces.Length - 1] * edgeNormals[faces.Length - 1] - bisectorDistances[0] * edgeNormals[0]);            
            for (int j = 1; j < faces.Length; j++)
            {
                result += A[faces[j]]*(bisectorDistances[j - 1]*edgeNormals[j - 1] - bisectorDistances[j]*edgeNormals[j]);
            }

            return result / _areas[vertex];
        }
        #endregion

        public ScalarField<Face> Divergence(VectorField<Face> field)
        {
            throw new NotImplementedException();
        }

        public VectorField<Face> Curl(VectorField<Face> field)
        {
            throw new NotImplementedException();
        }
    }
}
