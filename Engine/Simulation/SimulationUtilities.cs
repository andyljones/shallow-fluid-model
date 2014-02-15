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

        public static ScalarField<Face> CoriolisField(IPolyhedron surface, double rotationFrequency)
        {
            var angularVelocity = 2*Math.PI*rotationFrequency;
            var values = surface.Faces.Select(face => 2*angularVelocity*face.SphericalCenter().Normalize()[2]).ToArray();

            return new ScalarField<Face>(surface.IndexOf, values);
        }

        public static VectorField<Face> FaceNormalsField(IPolyhedron surface)
        {
            var values = surface.Faces.Select(face => face.SphericalCenter().Normalize()).ToArray();

            return new VectorField<Face>(surface.IndexOf, values);
        }

        public static VectorField<Vertex> VertexNormalsField(IPolyhedron surface)
        {
            var values = surface.Vertices.Select(vertex => vertex.Position.Normalize()).ToArray();

            return new VectorField<Vertex>(surface.IndexOf, values);
        }
    }
}
