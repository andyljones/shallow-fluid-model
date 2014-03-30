namespace Engine.Geometry
{
    /// <summary>
    /// Options for constructing an IPolyhedron for the simulation.
    /// </summary>
    public interface IPolyhedronOptions
    {
        double Radius { get; }
        int MinimumNumberOfFaces { get; }
    }
}
