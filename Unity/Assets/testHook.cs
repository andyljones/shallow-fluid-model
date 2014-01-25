using Assets;
using Assets.Rendering;
using Engine.Icosasphere;
using UnityEngine;
using System.Collections;

public class testHook : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
	    var polyhedron = new Icosahedron();
	    new PolyhedronRenderer(polyhedron, "Test", "Materials/TestMaterial", "Materials/TestMaterial2");
	}
}
