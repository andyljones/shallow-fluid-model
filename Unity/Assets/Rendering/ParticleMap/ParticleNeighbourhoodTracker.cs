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

        public ParticleNeighbourhoodTracker(IPolyhedron polyhedron)
        {
            var vertices = GetVertexPositions(polyhedron);
            _vertexTree = KDTree.MakeFromPoints(vertices);
        }

        private static Vector3[] GetVertexPositions(IPolyhedron surface)
        {
            return surface.Vertices.Select(vertex => GraphicsUtilities.Vector3(vertex.Position)).ToArray();
        }

        public int[] GetIndicesOfVerticesNearest(Vector3 position)
        {
            return new[] {_vertexTree.FindNearest(position)};
        }

    }
}
