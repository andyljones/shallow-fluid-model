using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Engine.Models;
using Engine.Polyhedra;
using Engine.Utilities;
using UnityEngine;

namespace Assets.Rendering.ParticleMap
{
    public class ParticleNeighbourhoodTracker
    {
        private readonly KDTree _vertexTree;
        private readonly Vector3[] _vertices;
        private readonly int[][] _indicesOfNeighbours;
        private readonly Vector3[][] _neighbours;
        private readonly int[][] _indicesOfVertexNeighbourhoods;

        private int[] _indicesOfNearestVertex;
        private int[][] _indicesOfNeighbourhood;

        public ParticleNeighbourhoodTracker(IPolyhedron polyhedron, int particleCount)
        {
            _vertices = GetVertexPositions(polyhedron);

            _vertexTree = KDTree.MakeFromPoints(_vertices);
            _indicesOfNeighbours = VertexIndexedTableFactory.Neighbours(polyhedron);
            _indicesOfVertexNeighbourhoods = BuildNeighbourhoodsTable(_indicesOfNeighbours);
            _neighbours = BuildVertexNeighbourTable(_indicesOfNeighbours, _vertices);

            _indicesOfNearestVertex = new int[particleCount];
            _indicesOfNeighbourhood = new int[particleCount][];
        }

        private static int[][] BuildNeighbourhoodsTable(int[][] indicesOfNeighbours)
        {
            var neighbourhoods = new int[indicesOfNeighbours.Length][];
            for (int i = 0; i < indicesOfNeighbours.Length; i++)
            {
                neighbourhoods[i] = indicesOfNeighbours[i].Concat(new[] {i}).ToArray();
            }

            return neighbourhoods;
        }

        private static Vector3[][] BuildVertexNeighbourTable(int[][] neighbourIndices, Vector3[] vertices)
        {
            var neighbours = new Vector3[vertices.Length][];
            for (int i = 0; i < vertices.Length; i++)
            {
                neighbours[i] = neighbourIndices[i].Select(j => vertices[j]).ToArray();
            }

            return neighbours;
        }

        private static Vector3[] GetVertexPositions(IPolyhedron surface)
        {
            return surface.Vertices.Select(vertex => GraphicsUtilities.Vector3(vertex.Position)).ToArray();
        }

        public int[][] GetIndicesOfVerticesNearest(Vector3[] particlePositions)
        {
            for (int i = 0; i < particlePositions.Length; i++)
            {
                var indexOfNearestVertex = GetIndexOfNearest(i, particlePositions[i]);
                _indicesOfNearestVertex[i] = indexOfNearestVertex;
                _indicesOfNeighbourhood[i] = _indicesOfVertexNeighbourhoods[indexOfNearestVertex];
            }

            return _indicesOfNeighbourhood;
        }

        private int GetIndexOfNearest(int particleIndex, Vector3 particlePosition)
        {
            var indexOfPreviousClosestVertex = _indicesOfNearestVertex[particleIndex];

            int indexOfNewClosestVertex;
            if (CheckIfAnyNeighbourIsCloser(particlePosition, indexOfPreviousClosestVertex, out indexOfNewClosestVertex))
            {
                int dummyOutVariable;
                if (CheckIfAnyNeighbourIsCloser(particlePosition, indexOfNewClosestVertex, out dummyOutVariable))
                {
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
