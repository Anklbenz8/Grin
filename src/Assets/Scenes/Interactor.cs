using System;
using UnityEngine;

namespace Scenes
{
    public class Interactor : MonoBehaviour
    {
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private Transform _camera;
        [SerializeField] private float _interactDistance;
        [SerializeField] private Vector3 _boxCastSize = new(0.1f, 0.1f, 0.1f);

        public void Interact(InteractionContext context)
        {
            var cameraTransform = _camera.transform;

            if (!Physics.BoxCast(cameraTransform.position, _boxCastSize, cameraTransform.forward, out RaycastHit hit,
                    Quaternion.identity, _interactDistance,
                    _layerMask))
                return;
            var interactable = hit.collider.GetComponentInParent<IInteractable>();
            interactable?.Interact(context);
        }

        private void OnDrawGizmos()
        {
            if (_camera == null) return;

            var camTransform = _camera.transform;

            // Начало бокса
            Vector3 origin = camTransform.position;
            Quaternion orientation = Quaternion.identity;
            Vector3 halfExtents = _boxCastSize;

            // Конец (смещаем на forward * distance)
            Vector3 end = origin + camTransform.forward * _interactDistance;

            Gizmos.color = Color.green;
            // Начальный бокс
            Gizmos.matrix = Matrix4x4.TRS(origin, orientation, Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, halfExtents * 2);

            Gizmos.color = Color.red;
            // Конечный бокс
            Gizmos.matrix = Matrix4x4.TRS(end, orientation, Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, halfExtents * 2);

            // Линия направления
            Gizmos.color = Color.yellow;
            Gizmos.matrix = Matrix4x4.identity;
            Gizmos.DrawLine(origin, end);
        }
    }
}