using UnityEngine;

namespace Scenes {
    public class InteractionContext {
        // что передает игрок объекту с которым взаимодействует
        
        public Vector3 forward; // для замков которые закрыты с одной из строн 
        // public Transform carryTransform; //трасформ для переносимых вещей 
        // public Invenroty invetory; //для инвентаря ключи и прочие замки
    }
    
    public class Interactor : MonoBehaviour {
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private Transform _camera;
        [SerializeField] private float _interactDistance;
        [SerializeField] private Vector3 _boxCastSize = new(0.1f, 0.1f, 0.1f);

        public void Interact(InteractionContext context) {
            var cameraTransform = _camera.transform;

            if (!Physics.BoxCast(cameraTransform.position, _boxCastSize, cameraTransform.forward, out RaycastHit hit, Quaternion.identity, _interactDistance,
                    _layerMask))
                return;
            var interactable = hit.collider.GetComponent<IInteractable>();
            interactable?.Interact(context);
        }
    }
}