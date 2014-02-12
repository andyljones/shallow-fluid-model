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
        private readonly double[] _vertexAreas;
        private readonly double[][] _areaInEachFace;
        private readonly double[][] _halfEdgeLengths;        
        private readonly Vector[][] _edgeNormals;

        private readonly double[][] _areaInEachVertex;
        private readonly int[][] _faceInFacesOfVertices;
        private readonly int[][] _vertices;
        private readonly double[] _faceAreas;

        private readonly Vector[][] _gradientCoefficients;
        private readonly Vector[][] _curlCoefficients;

        public VectorFieldOperators(IPolyhedron polyhedron)
        {
            _polyhedron = polyhedron;

            _edgeNormals = VertexIndexedTableFactory.EdgeNormals(polyhedron);
            _halfEdgeLengths = VertexIndexedTableFactory.HalfEdgeLengths(polyhedron);
            _faces = VertexIndexedTableFactory.Faces(polyhedron);
            _vertexAreas = VertexIndexedTableFactory.Areas(polyhedron);
            _areaInEachFace = VertexIndexedTableFactory.AreaInEachFace(polyhedron);

            _faceAreas = FaceIndexedTableFactory.Areas(polyhedron);
            _areaInEachVertex = FaceIndexedTableFactory.AreaInEachVertex(polyhedron);
            _faceInFacesOfVertices = FaceIndexedTableFactory.FaceInFacesOfVertices(polyhedron);
            _vertices = FaceIndexedTableFactory.Vertices(polyhedron);

            _gradientCoefficients = GradientPrecomputation();
            _curlCoefficients = CurlPrecomputation();
        }

        private Vector[][] NormalsTimesWidths()
        {
            var results = new Vector[_polyhedron.Vertices.Count][];
            for (int i = 0; i < _polyhedron.Vertices.Count; i++)
            {
                var lengths = _halfEdgeLengths[i];
                var normals = _edgeNormals[i];
                var result = new Vector[lengths.Length];
                for (int j = 0; j < lengths.Length; j++)
                {
                    result[j] = lengths[j] * normals[j];
                }
                results[i] = result;
            }

            return results;
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
            var coefficients = _gradientCoefficients[vertex];

            var result = Vector.Zeros(3);
            for (int j = 0; j < faces.Length; j++)
            {
                result += A[faces[j]]*coefficients[j];
            }

            return result / _vertexAreas[vertex];
        }

        private Vector[][] GradientPrecomputation()
        {
            var results = new Vector[_polyhedron.Vertices.Count][];
            for (int i = 0; i < _polyhedron.Vertices.Count; i++)
            {
                var lengths = _halfEdgeLengths[i];
                var normals = _edgeNormals[i];
                var result = new Vector[lengths.Length];
                for (int j = 0; j < lengths.Length; j++)
                {
                    result[j] = lengths.AtCyclicIndex(j-1)*normals.AtCyclicIndex(j-1) - lengths[j] * normals[j];
                }
                results[i] = result;
            }

            return results;
        }
        #endregion

        #region Divergence methods
        /// <summary>
        /// The discrete divergence, as described in Randall & Ringler 2001 (p7)
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

            return result / _faceAreas[face];
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
        /// The discrete curl, as described in Randall & Ringler 2001 (p7)
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

            return result/_faceAreas[face];
        }

        private Vector ContributionOfVertexToCurlAtFace(int vertex, int indexOfFaceInVertex, VectorField<Vertex> V)
        {
            return Vector.CrossProduct(_curlCoefficients[vertex][indexOfFaceInVertex], V[vertex]);
        }

        private Vector[][] CurlPrecomputation()
        {
            var results = new Vector[_polyhedron.Vertices.Count][];
            for (int i = 0; i < _polyhedron.Vertices.Count; i++)
            {
                var lengths = _halfEdgeLengths[i];
                var normals = _edgeNormals[i];
                var result = new Vector[lengths.Length];
                for (int j = 0; j < lengths.Length; j++)
                {
                    result[j] = lengths.AtCyclicIndex(j - 1) * normals.AtCyclicIndex(j - 1) + lengths[j] * normals[j];
                }
                results[i] = result;
            }

            return results;
        }
        #endregion

        #region VertexAverages methods.
        /// <summary>
        /// The vertex averaging operator, as described in Randall & Ringler 2001 (p16)
        /// </summary>
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
            var areaPerFace = _areaInEachFace[vertex];

            var weightedSum = 0.0;
            var totalArea = 0.0;
            for (int index = 0; index < faces.Length; index++)
            {
                weightedSum += F[faces[index]]*areaPerFace[index];
                totalArea += areaPerFace[index];
            }
            
            return weightedSum / totalArea;
        }
        #endregion

        #region Kinetic energy
        /// <summary>
        /// The kinetic energy, as described in Randall & Ringler 2001 (p44)
        /// </summary>
        public ScalarField<Face> KineticEnergy(VectorField<Vertex> V)
        {
            var numberOfFaces = _polyhedron.Faces.Count;

            var results = new double[numberOfFaces];
            for (int face = 0; face < numberOfFaces; face++)
            {
                results[face] = KineticEnergyAtFace(face, V);
            }

            return new ScalarField<Face>(_polyhedron.IndexOf, results);
        }

        private double KineticEnergyAtFace(int face, VectorField<Vertex> V)
        {
            var vertices = _vertices[face];
            var areasInVertices = _areaInEachVertex[face];

            var result = 0.0;
            for (int index = 0; index < vertices.Length; index++)
            {
                var vertex = vertices[index];
                result += areasInVertices[index]*Vector.ScalarProduct(V[vertex], V[vertex])/2;
            }

            return result/_faceAreas[face];
        }
        #endregion
    }
}
