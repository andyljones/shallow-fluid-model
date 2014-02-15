using System;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;

namespace Engine.Geometry
{
    public static class FaceUtilities
    {
        /// <summary>
        /// Calculates the area of a planar convex polygonal face. 
        /// </summary>
        public static double Area(this Face face)
        {
            var vectors = face.Vertices.Select(v => v.Position).ToArray();
            var flatArea = 0.0; 
            for (int i = 1; i < vectors.Length - 1; i++)
            {
                flatArea += Vector.CrossProduct(vectors[i] - vectors[0], vectors[i + 1] - vectors[0]).Norm()/2;
            }

            return Math.Abs(flatArea);
        }

        /// <summary>
        /// Calculates the center of a polygonal face then projects it onto the sphere the face is embedded in.
        /// 
        /// Degenerate for faces whose centroid is the origin.
        /// </summary>
        public static Vector SphericalCenter(this Face face)
        {
            var vectors = face.Vertices.Select(v => v.Position).ToArray();

            var center = face.Center();
            var radius = vectors.Average(vector => vector.Norm());

            return radius*center.Normalize();
        }

        public static Vector Center(this Face face)
        {
            var vectors = face.Vertices.Select(v => v.Position).ToArray();
            var centroid = Vector.Zeros(3);
            var area = 0.0;
            for (int i = 1; i < vectors.Length - 1; i++)
            {
                var centroidOfTriangle = (vectors[0] + vectors[i] + vectors[i + 1]) / 3;
                var areaOfTriangle = Vector.CrossProduct(vectors[i] - vectors[0], vectors[i + 1] - vectors[0]).Norm() / 2;
                centroid += areaOfTriangle * centroidOfTriangle;
                area += areaOfTriangle;
            }
            centroid = centroid / area;

            return centroid;
        }
    }
}
