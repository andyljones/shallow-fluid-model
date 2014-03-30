using System.Linq;
using System.Threading.Tasks;
using Engine.Geometry;
using Engine.Simulation;
using UnityEngine;

namespace Assets.Views.Level.ParticleMap
{
    /// <summary>
    /// Keeps track of the vertex nearest to each particle. Defaults to using local search from the last known closest
    /// vertex, but delegates to a K-D tree if a nearest vertex isn't found within a few steps.
    /// </summary>
    public class ParticleNeighbourhoodTracker
    {
        private readonly KDTree _vertexTree;
        private readonly Vector3[] _vertices;
        private readonly int[][] _indicesOfNeighbours;
        private readonly Vector3[][] _neighbours;
        private readonly int[][] _indicesOfVertexNeighbourhoods;

        private int[] _indicesOfNearestVertex;
        private int[][] _indicesOfNeighbourhood;

        /// <summary>
        /// Construct a tracker for the specified geometry and the specified number of vertices.
        /// </summary>
        public ParticleNeighbourhoodTracker(IPolyhedron polyhedron, int particleCount)
        {
            // Constructs KD tree to use to locate nearest vertices when local search fails.
            _vertices = GetVertexPositions(polyhedron);
            _vertexTree = KDTree.MakeFromPoints(_vertices);

            // Construct a lookup table that maps vertex indices to their neighbouring vertices.
            _indicesOfNeighbours = VertexIndexedTableFactory.Neighbours(polyhedron);
            _indicesOfVertexNeighbourhoods = BuildNeighbourhoodsTable(_indicesOfNeighbours);
            _neighbours = BuildVertexNeighbourTable(_indicesOfNeighbours, _vertices);

            // Construct a lookup table that takes a particle index to the index of its last known closest vertex,
            // and to the indices of the neighbouring vertices.
            _indicesOfNearestVertex = new int[particleCount];
            _indicesOfNeighbourhood = new int[particleCount][];
        }

        // Uses a table that maps the index of a vertex to the indices of its neighbours to build a new table which 
        // maps the index of a vertex to the indices of its neighbours plus its own index.
        private static int[][] BuildNeighbourhoodsTable(int[][] indicesOfNeighbours)
        {
            var neighbourhoods = new int[indicesOfNeighbours.Length][];
            for (int i = 0; i < indicesOfNeighbours.Length; i++)
            {
                neighbourhoods[i] = indicesOfNeighbours[i].Concat(new[] { i }).ToArray();
            }

            return neighbourhoods;
        }

        // Constructs a table that maps the index of a vertex to the positions of its neighbours.
        private static Vector3[][] BuildVertexNeighbourTable(int[][] indicesOfNeighbours, Vector3[] vertices)
        {
            var neighbours = new Vector3[vertices.Length][];
            for (int i = 0; i < vertices.Length; i++)
            {
                neighbours[i] = indicesOfNeighbours[i].Select(j => vertices[j]).ToArray();
            }

            return neighbours;
        }

        // Extracts the positions of the vertices from a surface.
        private static Vector3[] GetVertexPositions(IPolyhedron surface)
        {
            return surface.Vertices.Select(vertex => GraphicsUtilities.Vector3(vertex.Position)).ToArray();
        }

        /// <summary>
        /// Returns the indices of the nearest vertices to each particle. 
        /// </summary>
        /// <param name="particlePositions"></param>
        /// <returns></returns>
        public int[][] GetIndicesOfVerticesNearest(Vector3[] particlePositions)
        {
            for (int i = 0; i < particlePositions.Length; i++)
            {
                UpdateNearestVerticesAndNeighbourhoods(particlePositions, i);
            }

            //TODO: Should this be returned when its state has been modified by the function?
            return _indicesOfNeighbourhood;
        }

        //TODO: Why is this being passed an array and an index into an array? Would it be better to use a lazy getter?
        // Find the closest vertex to each particle, and 
        private void UpdateNearestVerticesAndNeighbourhoods(Vector3[] particlePositions, int i)
        {
            var indexOfNearestVertex = GetIndexOfNearest(i, particlePositions[i]);
            _indicesOfNearestVertex[i] = indexOfNearestVertex;
            _indicesOfNeighbourhood[i] = _indicesOfVertexNeighbourhoods[indexOfNearestVertex];
        }

        //TODO: Should pass the particleIndex and particlePosition in a single structure.
        // Finds the index of the vertex nearest the specified particle. Conducts a one-step local search, and if 
        // that fails to find the nearest vertex then it delegates to a K-D tree.
        private int GetIndexOfNearest(int particleIndex, Vector3 particlePosition)
        {
            // Start with the last known closest vertex
            var indexOfPreviousClosestVertex = _indicesOfNearestVertex[particleIndex];

            // See if any of the neighbours of the last known closest vertex are closer than it.
            int indexOfNewClosestVertex;
            if (CheckIfAnyNeighbourIsCloser(particlePosition, indexOfPreviousClosestVertex, out indexOfNewClosestVertex))
            {
                // If one is, check whether any of *its* neighbours are closer than it is. The dummy variable is 
                // because we have to pass *something* in order to find out whether NewClosestVertex is in fact closest.
                int dummyOutVariable;
                if (CheckIfAnyNeighbourIsCloser(particlePosition, indexOfNewClosestVertex, out dummyOutVariable))
                {
                    // If there are, then delegate to the KD-tree to find the nearest vertex.
                    indexOfNewClosestVertex = _vertexTree.FindNearest(particlePosition);
                }
            }

            return indexOfNewClosestVertex;
        }

        private bool CheckIfAnyNeighbourIsCloser(Vector3 particlePosition, int indexOfPreviousClosestVertex, out int indexOfNewClosestVertex)
        {
            var nearestVertex = _vertices[indexOfPreviousClosestVertex];
            var neighbours = _neighbours[indexOfPreviousClosestVertex];
            var indicesOfNeighbours = _indicesOfNeighbours[indexOfPreviousClosestVertex];

            var aNeighbourIsCloser = false;
            indexOfNewClosestVertex = indexOfPreviousClosestVertex;
            var currentSimilarity = Vector3.Dot(particlePosition, nearestVertex);
            for (int j = 0; j < neighbours.Length; j++)
            {
                var neighbourSimilarity = Vector3.Dot(particlePosition, neighbours[j]);

                if (neighbourSimilarity > currentSimilarity)
                {
                    aNeighbourIsCloser = true;
                    indexOfNewClosestVertex = indicesOfNeighbours[j];
                    currentSimilarity = neighbourSimilarity;
                }
            }

            return aNeighbourIsCloser;
        }

    }
}
