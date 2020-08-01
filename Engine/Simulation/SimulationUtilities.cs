using System;
using System.Linq;
using Engine.Geometry;

namespace Engine.Simulation
{
    /// <summary>
    /// A set of static utility methods for use by simulation classes.
    /// </summary>
    public static class SimulationUtilities
    {
        /// <summary>
        /// Calculates the Coriolis acceleration field for the given surface rotating at the given frequency.
        /// </summary>
        /// <param name="surface"></param>
        /// <param name="rotationFrequency"></param>
        /// <returns></returns>
        public static ScalarField<Face> CoriolisField(IPolyhedron surface, double rotationFrequency)
        {
            var angularVelocity = 2*Math.PI*rotationFrequency;
            var values = surface.Faces.Select(face => 2*angularVelocity*face.SphericalCenter().Normalize(2)[2]).ToArray();

            return new ScalarField<Face>(surface.IndexOf, values);
        }

        /// <summary>
        /// Calculates the field of normals to each face.
        /// </summary>
        /// <param name="surface"></param>
        /// <returns></returns>
        public static VectorField<Face> FaceNormalsField(IPolyhedron surface)
        {
            var values = surface.Faces.Select(face => face.SphericalCenter().Normalize(2)).ToArray();

            return new VectorField<Face>(surface.IndexOf, values);
        }

        /// <summary>
        /// Calculates the field of normals to each vertex.
        /// </summary>
        /// <param name="surface"></param>
        /// <returns></returns>
        public static VectorField<Vertex> VertexNormalsField(IPolyhedron surface)
        {
            var values = surface.Vertices.Select(vertex => vertex.Position.Normalize(2)).ToArray();

            return new VectorField<Vertex>(surface.IndexOf, values);
        }
    }
}
