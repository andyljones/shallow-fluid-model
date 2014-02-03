using Engine.Models.MomentumModel;
using Engine.Polyhedra;

namespace Engine.Models.VorticityDivergenceModel
{
    public class VelocityFieldFactory
    {
        private VectorFieldOperators _operators;
        //private readonly int[][] _neighbours;
        //private readonly Vector[][] _directions;
        //private readonly double[][] _distances;

        private readonly VectorField<Vertex> _normals;

        public VelocityFieldFactory(IPolyhedron polyhedron)
        {
            //_neighbours = FaceIndexedTableFactory.Neighbours(polyhedron);
            //_directions = FaceIndexedTableFactory.Directions(polyhedron);
            //_distances = FaceIndexedTableFactory.Distances(polyhedron);

            _normals = new VectorField<Vertex>(polyhedron.IndexOf, VertexIndexedTableFactory.Normals(polyhedron));

            _operators = new VectorFieldOperators(polyhedron);
        }

        public VectorField<Vertex> VelocityField(ScalarField<Face> streamfunction, ScalarField<Face> vectorPotential)
        {
            var velocity = VectorField<Vertex>.CrossProduct(_normals, _operators.Gradient(streamfunction)) + _operators.Gradient(vectorPotential);

            return velocity;
        }

        //private VectorField<Face> Gradient(ScalarField<Face> A)
        //{
        //    var gradients = new Vector[A.Count];
        //    foreach (var face in Enumerable.Range(0, A.Count))
        //    {
        //        gradients[face] = GradientAtFace(face, A);
        //    }

        //    return new VectorField<Face>(A.IndexOf, gradients);
        //}

        //private Vector GradientAtFace(int face, ScalarField<Face> A)
        //{
        //    var neighbours = _neighbours[face];
        //    var directions = _directions[face];
        //    var distances = _distances[face];

        //    var gradient = Vector.Zeros(3);
        //    for (int i = 0; i < neighbours.Length; i++)
        //    {
        //        var neighbour = neighbours[i];
        //        gradient += (A[neighbour] - A[face])/distances[i]*directions[i];
        //    }

        //    return gradient/neighbours.Length;
        //}
    }
}
