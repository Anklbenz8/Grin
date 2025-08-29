using UnityEngine;

namespace Scenes {
    public class Mover : MonoBehaviour {
        [SerializeField] private CharacterController characterController;
        [SerializeField] private Camera camera;
        [SerializeField] private float jumpHeigth = 1f;
        [SerializeField] private float moveSpeed = 2f;
        [SerializeField] private float lookSensitivity = 3f;
        [SerializeField] private Vector2 lookUpMinMax = new Vector2(-80f, 80f);

        private float _cameraPitch = 0f;

        public void Move(Vector3 inputDirection) {
            Vector2 movement = inputDirection;
            var directionVector = transform.forward * movement.y + transform.right * movement.x;
            directionVector *= moveSpeed;
            characterController.SimpleMove(directionVector);
        }

        public void Look(Vector3 mouseDelta) {
            transform.Rotate(Vector3.up, mouseDelta.x * lookSensitivity);
            _cameraPitch -= mouseDelta.y * lookSensitivity;
            _cameraPitch = Mathf.Clamp(_cameraPitch, lookUpMinMax.x, lookUpMinMax.y);

            camera.transform.localEulerAngles = new Vector3(_cameraPitch, 0, 0);
        }

        public void Jump() {
            /*if (characterController.isGrounded) {
                var jumpVelocity = Mathf.Sqrt(-2 * jumpHeigth * -9.81f);
                characterController.Move(new Vector3(0, jumpVelocity, 0));
            }*/
        }
    }
}