using System.Diagnostics;
using System.Linq;
using Engine.Polyhedra;
using Engine.Utilities;
using MathNet.Numerics.LinearAlgebra;

namespace Engine.Models.MomentumModel
{
    public class VectorFieldOperators
    {
        private readonly IPolyhedron _polyhedron;

        private readonly int[][] _faces;
        private readonly double[] _areas;
        private readonly double[][] _halfEdgeLengths;        
        private readonly Vector[][] _edgeNormals;

        private readonly int[][] _faceInFacesOfVertices;
        private readonly int[][] _vertices;

        public VectorFieldOperators(IPolyhedron polyhedron)
        {
            _polyhedron = polyhedron;

            _edgeNormals = VertexIndexedTableFactory.EdgeNormals(polyhedron);
            _halfEdgeLengths = VertexIndexedTableFactory.HalfEdgeLengths(polyhedron);
            _faces = VertexIndexedTableFactory.Faces(polyhedron);
            _areas = VertexIndexedTableFactory.Areas(polyhedron);

            _faceInFacesOfVertices = FaceIndexedTableFactory.FaceInFacesOfVertices(polyhedron);
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
            var bisectorDistances = _halfEdgeLengths[vertex];
            var edgeNormals = _edgeNormals[vertex];

            var result = Vector.Zeros(3);
            for (int j = 0; j < faces.Length; j++)
            {

                result += A[faces[j]]*(bisectorDistances.AtCyclicIndex(j-1)*edgeNormals.AtCyclicIndex(j-1) - bisectorDistances[j]*edgeNormals[j]);
            }

            return result / _areas[vertex];
        }
        #endregion

        #region Divergence methods
        /// <summary>
        /// The discrete divergence, as described in Randall & Ringler 2001.
        /// </summary>
        public ScalarField<Face> FluxDivergence(VectorField<Vertex> V, ScalarField<Face> F)
        {
            var numberOfFaces = _polyhedron.Faces.Count;
            var fluxes = FluxesAcrossHalfEdges(V);

            var results = new double[numberOfFaces];
            for (int face = 0; face < numberOfFaces; face++)
            {
                results[face] = DivergenceAtFace(face, fluxes, F);
            }

            return new ScalarField<Face>(_polyhedron.IndexOf, results);
        }

        #region DivergenceAtFace methods
        private double DivergenceAtFace(int face, double[][] fluxes, ScalarField<Face> F)
        {
            var vertices = _vertices[face];
            var faceInFacesOfVertices = _faceInFacesOfVertices[face];

            var result = 0.0;
            for (int j = 0; j < faceInFacesOfVertices.Length; j++)
            {
                var vertex = vertices[j];
                var faceInFacesOfVertex = faceInFacesOfVertices[j];

                result += ContributionOfVertexToFluxAtFace(vertex, faceInFacesOfVertex, fluxes, F);
            }

            return result / _areas[face];
        }

        private double ContributionOfVertexToFluxAtFace(int vertex, int faceInFacesOfVertex, double[][] fluxes, ScalarField<Face> F)
        {
            var facesOfVertex = _faces[vertex];

            var indexOfThisFace = facesOfVertex.AtCyclicIndex(faceInFacesOfVertex);
            var valueAtThisFace = F[indexOfThisFace];

            var indexOfNextFace = facesOfVertex.AtCyclicIndex(faceInFacesOfVertex + 1);
            var valueAtNextFace = F[indexOfNextFace];
            var valueAtNextHalfEdge = (valueAtNextFace + valueAtThisFace) / 2;
            var fluxAtNextHalfEdge = fluxes[vertex][faceInFacesOfVertex];
            var fieldFluxAtNextHalfEdge = valueAtNextHalfEdge*fluxAtNextHalfEdge;

            var indexOfPreviousFace = facesOfVertex.AtCyclicIndex(faceInFacesOfVertex - 1);
            var valueAtPreviousFace = F[indexOfPreviousFace];
            var valueAtPreviousHalfEdge = (valueAtThisFace + valueAtPreviousFace) / 2;
            var fluxAtPreviousHalfEdge = -fluxes[vertex].AtCyclicIndex(faceInFacesOfVertex - 1);
            var fieldFluxAtPreviousHalfEdge = valueAtPreviousHalfEdge*fluxAtPreviousHalfEdge;

            return fieldFluxAtNextHalfEdge + fieldFluxAtPreviousHalfEdge;
        }
        #endregion

        #region FluxesAcrossHalfEdges methods
        private double[][] FluxesAcrossHalfEdges(VectorField<Vertex> V)
        {
            var numberOfVertices = _polyhedron.Vertices.Count;

            var fluxes = new double[numberOfVertices][];
            for (int vertex = 0; vertex < numberOfVertices; vertex++)
            {
                fluxes[vertex] = HalfEdgeFluxesAroundVertex(V[vertex], _edgeNormals[vertex], _halfEdgeLengths[vertex]);
            }

            return fluxes;
        }

        private double[] HalfEdgeFluxesAroundVertex(Vector v, Vector[] edgeNormals, double[] halfEdgeLengths)
        {
            var fluxes = new double[edgeNormals.Length];
            for (int halfEdge = 0; halfEdge < edgeNormals.Length; halfEdge++)
            {
                fluxes[halfEdge] = Vector.ScalarProduct(v, edgeNormals[halfEdge]) * halfEdgeLengths[halfEdge];
            }

            return fluxes;
        }
        #endregion
        #endregion

        #region Curl methods.
        /// <summary>
        /// The discrete curl, as described in Randall & Ringler 2001.
        /// </summary>
        public VectorField<Face> Curl(VectorField<Vertex> V)
        {
            var numberOfFaces = _polyhedron.Faces.Count;

            var results = new Vector[numberOfFaces];
            for (int face = 0; face < numberOfFaces; face++)
            {
                results[face] = CurlAtFace(face, V);
            }

            return new VectorField<Face>(_polyhedron.IndexOf, results);
        }

        private Vector CurlAtFace(int face, VectorField<Vertex> V)
        {
            var vertices = _vertices[face];
            var faceInFacesOfVertex = _faceInFacesOfVertices[face];

            var result = Vector.Zeros(3);
            for (int index = 0; index < vertices.Length; index++)
            {
                var indexOfFaceInVertex = faceInFacesOfVertex[index];
                result += ContributionOfVertexToCurlAtFace(vertices[index], indexOfFaceInVertex, V);
            }

            return result/_areas[face];
        }

        private Vector ContributionOfVertexToCurlAtFace(int vertex, int indexOfFaceInVertex, VectorField<Vertex> V)
        {
            var normalAtNextHalfEdge = _edgeNormals[vertex].AtCyclicIndex(indexOfFaceInVertex);
            var lengthOfNextHalfEdge = _halfEdgeLengths[vertex].AtCyclicIndex(indexOfFaceInVertex);

            var normalAtPreviousHalfEdge = _edgeNormals[vertex].AtCyclicIndex(indexOfFaceInVertex - 1);
            var lengthOfPreviousHalfEdge = _halfEdgeLengths[vertex].AtCyclicIndex(indexOfFaceInVertex - 1);

            var sumOfNormals = (normalAtNextHalfEdge * lengthOfNextHalfEdge + normalAtPreviousHalfEdge * lengthOfPreviousHalfEdge);

            return Vector.CrossProduct(sumOfNormals, V[vertex]);
        }
        #endregion

        #region VertexAverages methods.
        public ScalarField<Vertex> VertexAverages(ScalarField<Face> F)
        {
            var numberOfVertices = _polyhedron.Vertices.Count;

            var averages = new double[numberOfVertices];
            for (int vertex = 0; vertex < numberOfVertices; vertex++)
            {
                averages[vertex] = VertexAverage(vertex, F);
            }

            return new ScalarField<Vertex>(_polyhedron.IndexOf, averages);
        }

        private double VertexAverage(int vertex, ScalarField<Face> F)
        {
            var faces = _faces[vertex];

            var sum = 0.0;
            for (int index = 0; index < faces.Length; index++)
            {
                sum += F[faces[index]];
            }
            
            return sum / faces.Length;
        }
        #endregion

        #region Kinetic energy

        #endregion
    }
}
