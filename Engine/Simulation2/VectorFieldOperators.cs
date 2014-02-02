using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Polyhedra;
using Engine.Simulation;
using Engine.Utilities;
using MathNet.Numerics.LinearAlgebra;

namespace Engine.Simulation2
{
    public class VectorFieldOperators
    {
        private readonly IPolyhedron _polyhedron;
                 
        private readonly Vector[][] _edgeNormals;
        private readonly double[][] _bisectorDistances;
        private readonly int[][] _faces;
        private readonly double[] _areas;

        private readonly int[][] _facesInVertices;
        private readonly int[][] _vertices;
        
        public VectorFieldOperators(IPolyhedron polyhedron)
        {
            _polyhedron = polyhedron;

            _edgeNormals = VertexIndexedTableFactory.EdgeNormals(polyhedron);
            _bisectorDistances = VertexIndexedTableFactory.BisectorDistances(polyhedron);
            _faces = VertexIndexedTableFactory.Faces(polyhedron);
            _areas = VertexIndexedTableFactory.Areas(polyhedron);

            _facesInVertices = FaceIndexedTableFactory.FaceInVertices(polyhedron);
            _vertices = FaceIndexedTableFactory.Vertices(polyhedron);
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
            for (int j = 0; j < faces.Length; j++)
            {
                result += A[faces[j]]*(bisectorDistances.AtCyclicIndex(j-1)*edgeNormals.AtCyclicIndex(j-1) - bisectorDistances[j]*edgeNormals[j]);
            }

            return result / _areas[vertex];
        }
        #endregion

        public ScalarField<Face> Divergence(VectorField<Vertex> V, ScalarField<Face> F)
        {
            var numberOfFaces = _polyhedron.Faces.Count;
            var fluxes = HalfEdgeFluxes(V);

            var results = new double[numberOfFaces];
            for (int face = 0; face < numberOfFaces; face++)
            {
                results[face] = DivergenceAtFace(face, fluxes, F);
            }

            return new ScalarField<Face>(_polyhedron.IndexOf, results);
        }

        private double[][] HalfEdgeFluxes(VectorField<Vertex> V)
        {
            var numberOfVertices = _polyhedron.Vertices.Count;

            var fluxes = new double[numberOfVertices][];
            for (int i = 0; i < numberOfVertices; i++)
            {
                fluxes[i] = HalfEdgeFluxesAroundVertex(V[i], _edgeNormals[i], _bisectorDistances[i]);
            }

            return fluxes;
        }

        private double[] HalfEdgeFluxesAroundVertex(Vector v, Vector[] edgeNormals, double[] halfEdgeLengths)
        {
            var fluxesAroundVertex = new double[edgeNormals.Length];
            for (int j = 0; j < edgeNormals.Length; j++)
            {
                fluxesAroundVertex[j] = Vector.ScalarProduct(v, edgeNormals[j])*halfEdgeLengths[j];
            }

            return fluxesAroundVertex;
        }

        private double DivergenceAtFace(int face, double[][] fluxes, ScalarField<Face> F)
        {
            var vertices = _vertices[face];
            var indexOfFaceInVertices = _facesInVertices[face];

            var result = 0.0;
            for (int j = 0; j < indexOfFaceInVertices.Length; j++)
            {
                var thisVertex = vertices[j];
                var indexOfFaceInVertex = indexOfFaceInVertices[j];

                result += FieldFluxFromFaceAcrossVertex(thisVertex, indexOfFaceInVertex, fluxes, F);
            }

            return result / _areas[face];
        }

        private double FieldFluxFromFaceAcrossVertex(int vertex, int indexOfFaceInVertex, double[][] fluxes, ScalarField<Face> F)
        {
            var facesAroundVertex = _faces[vertex];
            var valueAtThisFace = F[facesAroundVertex.AtCyclicIndex(indexOfFaceInVertex)];

            var valueAtNextFace = F[facesAroundVertex.AtCyclicIndex(indexOfFaceInVertex + 1)];
            var valueAtThisEdge = (valueAtNextFace + valueAtThisFace) / 2;
            var fluxAtThisEdge = fluxes[vertex][indexOfFaceInVertex];

            var valueAtPreviousFace = F[facesAroundVertex.AtCyclicIndex(indexOfFaceInVertex - 1)];
            var valuesAtPreviousEdge = (valueAtThisFace + valueAtPreviousFace) / 2;
            var fluxAtPreviousEdge = -fluxes[vertex].AtCyclicIndex(indexOfFaceInVertex - 1);

            return valueAtThisEdge * fluxAtThisEdge + valuesAtPreviousEdge * fluxAtPreviousEdge;
        }
    }
}
