using UnityEngine;

namespace Scenes {
    public class Interactor : MonoBehaviour {
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private Transform _camera;
        [SerializeField] private float _interactDistance;
        [SerializeField] private Vector3 _boxCastSize = new(0.1f, 0.1f, 0.1f);

        public void Interact() {
            var cameraTransform = _camera.transform;

            if (!Physics.BoxCast(cameraTransform.position, _boxCastSize, cameraTransform.forward, out RaycastHit hit, Quaternion.identity, _interactDistance,
                    _layerMask))
                return;
            var interactable = hit.collider.GetComponent<IInteractable>();
            interactable?.Interact();
        }
    }
}