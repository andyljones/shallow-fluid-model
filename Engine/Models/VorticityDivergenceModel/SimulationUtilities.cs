using System;
using Engine.Polyhedra;

namespace Engine.Models.VorticityDivergenceModel
{
    /// <summary>
    /// A set of static utility methods for use by simulation classes.
    /// </summary>
    public static class SimulationUtilities
    {

        public static ScalarField<Face> CoriolisField(IPolyhedron surface, double rotationFrequency)
        {
            var angularVelocity = 2*Math.PI*rotationFrequency;

            var values = new double[surface.Faces.Count];
            foreach (var face in surface.Faces)
            {
                var faceIndex = surface.IndexOf(face);
                values[faceIndex] = 2*angularVelocity*face.SphericalCenter().Normalize()[2];
            }

            return new ScalarField<Face>(surface.IndexOf, values);
        }
    }
}
