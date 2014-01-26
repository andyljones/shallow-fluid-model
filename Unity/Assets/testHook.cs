using Assets.Rendering;
using Engine.Polyhedra.IcosahedronBased;
using UnityEngine;

namespace Assets
{
    public class TestHook : MonoBehaviour {

        // Use this for initialization
        void Start ()
        {
            var polyhedron = GeodesicSphereFactory.Build(43);
            new PolyhedronRenderer(polyhedron, "Test2", "Materials/TestMaterial", "Materials/TestMaterial2");
            Debug.Log(polyhedron.Vertices.Count);
            Debug.Log(polyhedron.Edges.Count);
            Debug.Log(polyhedron.Faces.Count);
        }
    }
}
