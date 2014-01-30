using System.Linq;
using Engine.Polyhedra;
using Engine.Simulation;
using UnityEngine;

namespace Assets.Rendering
{
    public class ColorMap
    {
        private readonly MeshFilter _meshFilter;
        private readonly IPolyhedron _polyhedron;

        public ColorMap(GameObject surface, IPolyhedron polyhedron)
        {
            _meshFilter = surface.GetComponent<MeshFilter>();
            _polyhedron = polyhedron;
        }

        public void Update(ScalarField<Face> field)
        {
            var averages = _polyhedron.Vertices.Select(vertex => AverageAt(vertex, field)).ToArray();
            var max = averages.Max();
            var min = averages.Min();
            var normalizedValues = averages.Select(average => (average - min)/(max - min)).ToArray();
            var colors = normalizedValues.Select(average => Color.Lerp(Color.green, Color.red, (float)average)).ToArray();

            _meshFilter.mesh.colors = colors;
        }

        private double AverageAt(Vertex vertex, ScalarField<Face> field)
        {
            return _polyhedron.FacesOf(vertex).Average(face => field[face]);
        }


    }
}
