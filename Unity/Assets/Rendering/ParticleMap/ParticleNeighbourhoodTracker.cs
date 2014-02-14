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
        private readonly int[][] _neighbourIndices;
        private readonly Vector3[][] _neighbours;

        private int[] _indicesOfNearestVertex;

        public ParticleNeighbourhoodTracker(IPolyhedron polyhedron, int particleCount)
        {
            _vertices = GetVertexPositions(polyhedron);

            _vertexTree = KDTree.MakeFromPoints(_vertices);
            _neighbourIndices = VertexIndexedTableFactory.Neighbours(polyhedron);
            _neighbours = BuildVertexNeighbourTable(_neighbourIndices, _vertices);

            _indicesOfNearestVertex = new int[particleCount];
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

        public int[] GetIndicesOfVerticesNearest(Vector3[] particlePositions)
        {
            for (int i = 0; i < particlePositions.Length; i++)
            {
                _indicesOfNearestVertex[i] = GetIndexOfNearest(i, particlePositions[i]);
            }

            return _indicesOfNearestVertex;
        }

        private int GetIndexOfNearest(int particleIndex, Vector3 particlePosition)
        {
            var indexOfPreviousNearestVertex = _indicesOfNearestVertex[particleIndex];

            if (CheckIfAnyNeighbourIsCloser(particlePosition, indexOfPreviousNearestVertex))
            {
                return _vertexTree.FindNearest(particlePosition);
            }
            else
            {
                return indexOfPreviousNearestVertex;
            }
        }

        private bool CheckIfAnyNeighbourIsCloser(Vector3 particlePosition, int indexOfPreviousNearestVertex)
        {
            var previousNearestVertex = _vertices[indexOfPreviousNearestVertex];
            var previousNeighbours = _neighbours[indexOfPreviousNearestVertex];

            var sqrDistanceToNearest = (particlePosition - previousNearestVertex).sqrMagnitude;
            for (int j = 0; j < previousNeighbours.Length; j++)
            {
                var sqrDistanceToNeighbour = (particlePosition - previousNeighbours[j]).sqrMagnitude;

                if (sqrDistanceToNeighbour < sqrDistanceToNearest)
                {
                    return true;
                }
            }

            return false;
        }

    }
}
