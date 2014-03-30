namespace Engine.Geometry
{
    /// <summary>
    /// Options required by the Polyhedron class.
    /// </summary>
    public class PolyhedronOptions : IPolyhedronOptions
    {
        public double Radius { get; set; }
        public int MinimumNumberOfFaces { get; set; }
    }
}
