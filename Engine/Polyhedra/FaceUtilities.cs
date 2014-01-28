using System;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;

namespace Engine.Polyhedra
{
    public static class FaceUtilities
    {
        /// <summary>
        /// Calculates the area of a polygonal face. 
        /// </summary>
        public static double Area(this Face face)
        {
            var vectors = face.Vertices.Select(v => v.Position.Normalize()).ToArray();
            var normalToPlane = Vector.CrossProduct(vectors[0] - vectors[1], vectors[2] - vectors[1]);
            var flatArea = 0.0; 
            for (int i = 0; i < vectors.Length - 1; i++)
            {
                flatArea += Vector.ScalarProduct(normalToPlane, Vector.CrossProduct(vectors[i], vectors[i + 1]));
            }
            flatArea += Vector.ScalarProduct(normalToPlane, Vector.CrossProduct(vectors[vectors.Length - 1], vectors[0]));

            //TODO: Should calculate the area of the section of the sphere subtended.

            return flatArea;
        }

        /// <summary>
        /// Calculates the center of the spherical cap of a polygonal face. Is accurate when the face is planar.
        /// </summary>
        public static Vector Center(this Face face)
        {
            var centroid = face.Vertices.Aggregate(Vector.Zeros(3), (vector, vertex) => vector + vertex.Position)/face.Vertices.Count;
            var averageRadius = face.Vertices.Average(vertex => vertex.Position.Norm());

            return averageRadius*centroid.Normalize();
        }
    }
}
