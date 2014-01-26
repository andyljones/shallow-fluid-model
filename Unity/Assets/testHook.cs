using Assets.Rendering;
using Engine;
using Engine.Polyhedra.IcosahedronBased;
using UnityEngine;

namespace Assets
{
    public class TestHook : MonoBehaviour {

        // Use this for initialization
        void Start ()
        {
            var options = new Options {MinimumNumberOfFaces = 13, Radius = 1};
            var polyhedron = GeodesicSphereFactory.Build(options);
            new PolyhedronRenderer(polyhedron, "Test2", "Materials/TestMaterial", "Materials/TestMaterial2");
            Debug.Log(polyhedron.Vertices.Count);
            Debug.Log(polyhedron.Edges.Count);
            Debug.Log(polyhedron.Faces.Count);
        }
    }
}
