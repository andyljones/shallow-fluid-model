﻿using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;
using Engine.Utilities;

namespace Engine.Geometry.GeodesicSphere
{

    /// <summary>
    /// Factory for geodesic spheres (spheres comprised of 12 pentagons and some number of hexagons)
    /// </summary>
    public static class GeodesicSphereFactory
    {
        // Icosaspheres are generated by repeatedly subdividing the faces of an icosahedron, then projecting the resultant
        // vertices onto the sphere.

        /// <summary>
        /// Constructs the geodesic sphere with a number of faces exceeding the specified minimum. 
        /// </summary>
        public static IPolyhedron Build(IPolyhedronOptions options)
        {
            // Build an icosasphere with a minimum number of faces. 
            var minimumNumberOfVertices = 2*options.MinimumNumberOfFaces - 4;
            var icosasphereOptions = new PolyhedronOptions
            {
                Radius = options.Radius,
                MinimumNumberOfFaces = minimumNumberOfVertices
            };
            var icosasphere = IcosasphereFactory.Build(icosasphereOptions);
            
            // Take the icosasphere's dual to get the geodesic sphere we want.
            var faces = DualofIcosasphere(icosasphere);
            return new Polyhedron(faces);
        }

        // Constructs the dual of the provided icosasphere, returning a list of lists in which each list represents the
        // vertices of a face.
        private static List<List<Vertex>> DualofIcosasphere(IPolyhedron icosasphere)
        {
            // Map each icosasphere face to a geodesic sphere vertex
            var newVertexDict = icosasphere.Faces.ToDictionary(face => face, face => VertexAtCenterOf(face));
            // Gather the new vertices into lists that represent the faces of the geodesic sphere.
            var vertexLists = 
                icosasphere.Vertices.
                Select(oldVertex => CreateFaceAbout(oldVertex, newVertexDict, icosasphere.FacesOf)).ToList();

            return vertexLists;
        }

        // Gets the list of geodesic sphere vertices that correspond to the faces surrounding a vertex of the icosaphere.
        private static List<Vertex> CreateFaceAbout(Vertex oldVertex, Dictionary<Face, Vertex> newVertexDict, Func<Vertex, List<Face>> oldFacesDict)
        {
            var oldFaces = oldFacesDict(oldVertex);
            var newVertices = oldFaces.Select(oldFace => newVertexDict[oldFace]).ToList();

            return newVertices;
        }

        // Creates a point at the center of the provided face.
        private static Vertex VertexAtCenterOf(Face face)
        {
            var a = face.Vertices[0].Position;
            var b = face.Vertices[1].Position;
            var c = face.Vertices[2].Position;

            var radius = (a.Norm(2) + b.Norm(2) + c.Norm(2))/3;

            var center = radius*VectorUtilities.CrossProduct(a - b, c - b).Normalize(2);

            return new Vertex(center);
        }
    }
}
