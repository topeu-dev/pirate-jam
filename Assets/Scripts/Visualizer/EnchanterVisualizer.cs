using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Visualizer
{
    public class EnchanterVisualizer : MonoBehaviour
    {
        private DecalProjector _decalProjector;

        public Vector3 targetRotation = new Vector3(0, 360, 0); // Target rotation in degrees.
        public float rotationSpeed = 1.0f;

        private bool _enabled = false;
        private float _radius = 4f;

        private void Awake()
        {
            _decalProjector = GetComponent<DecalProjector>();
        }

        private void Update()
        {
            if (!_enabled)
                return;
            Debug.Log("EnchanterVisualizer - enabled");
            _decalProjector.size = new Vector3(9, 9, _decalProjector.size.z);
            _decalProjector.enabled = true;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out RaycastHit hit, 200f, 1 << LayerMask.NameToLayer("Field")))
                return;

            _decalProjector.transform.position = new Vector3(
                hit.point.x,
                _decalProjector.transform.position.y,
                hit.point.z);

            // Quaternion currentRotation = _decalProjector.transform.rotation;
            // Quaternion targetQuat = Quaternion.Euler(targetRotation);
            // _decalProjector.transform.rotation =
            //     Quaternion.Lerp(currentRotation, targetQuat, rotationSpeed * Time.deltaTime);
        }

        public void EnableVisualizer()
        {
            _decalProjector.enabled = true;
            _enabled = true;
        }

        public void DisableVisualizer()
        {
            _decalProjector.enabled = false;
            _enabled = false;
        }
    }
}