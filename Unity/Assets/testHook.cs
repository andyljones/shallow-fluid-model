using Assets.Rendering;
using Assets.UserInterface;
using Engine;
using Engine.Models;
using Engine.Models.MomentumModel;
using Engine.Polyhedra;
using Engine.Polyhedra.IcosahedronBased;
using UnityEngine;

namespace Assets
{
    public class TestHook : MonoBehaviour
    {
        private IPolyhedron _polyhedron;

        private PolyhedronRenderer _polyhedronRenderer;
        private PrognosticFieldsUpdater _updater;

        private CameraPositionController _cameraPositionController;
        private PrognosticFieldsFactory _fieldFactory;
        private PrognosticFields _fields;

        // Use this for initialization
        void Start ()
        {
            var options = new Options
            {
                MinimumNumberOfFaces = 100,
                Radius = 6000,
                SurfaceMaterialName = "Materials/Surface",
                WireframeMaterialName = "Materials/Wireframe"
            };

            _polyhedron = GeodesicSphereFactory.Build(options);

            var polyhedronMesh = new PolyhedronMesh(_polyhedron);
            _polyhedronRenderer = new PolyhedronRenderer(_polyhedron, polyhedronMesh.Mesh, options);
            _cameraPositionController = new CameraPositionController(9000, CameraFactory.Build());
        }

        void Update()
        {
        }

        void LateUpdate()
        {
            _cameraPositionController.LateUpdate();   
        }
    }
}
