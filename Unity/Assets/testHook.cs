using Assets.Rendering;
using Assets.UserInterface;
using Engine;
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

        // Use this for initialization
        void Start ()
        {
            var options = new Options {MinimumNumberOfFaces = 100, Radius = 6000};
            _polyhedron = GeodesicSphereFactory.Build(options);
            _polyhedronRenderer = new PolyhedronRenderer(_polyhedron, "Surface", "Materials/Wireframe", "Materials/Surface");
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
