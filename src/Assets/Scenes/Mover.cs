using UnityEngine;

namespace Scenes {
    public class Mover : MonoBehaviour {
        private const float GRAVITY = -9.81f;

        [SerializeField] private CharacterController characterController;
        [SerializeField] private Camera camera;
        [SerializeField] private float jumpHeight = 0.7f;
        [SerializeField] private float standHeight = 1.8f;
        [SerializeField] private float crouchHeight = 1f;
        [SerializeField] private float crouchSpeed = 6f;
        [SerializeField] private float moveSpeed = 2f;
        [SerializeField] private float strafeSpeed = 1f;
        [SerializeField] private float lookSensitivity = 3f;
        [SerializeField] private float crouchCameraOffset = 0.5f;
        [SerializeField] private float crouchSpeedMultiplier = 0.5f;
        [SerializeField] private Vector2 lookUpMinMax = new Vector2(-80f, 80f);

        private Vector3 _moveVector;
        private float _cameraPitch = 0f;
        private float _verticalVelocity;
        private bool _isGrounded;
        private bool _isCrouching;
        private bool _isJumpRequested;

        private void FixedUpdate() {
            HandleGravityAndJump();
            HandleMovement();
            HandleCrouch();
        }

        public void Move(Vector2 inputDirection) {
            Vector3 forward = transform.forward;
            Vector3 right = transform.right;
            // Нормализуем input, чтобы диагональная скорость не превышала скорость вперед/назад
            Vector2 inputNormalized = Vector2.ClampMagnitude(inputDirection, 1f);

            _moveVector = (forward * inputNormalized.y + right * inputNormalized.x) * moveSpeed;
        }

        public void Look(Vector3 mouseDelta) {
            transform.Rotate(Vector3.up, mouseDelta.x * lookSensitivity);
            _cameraPitch -= mouseDelta.y * lookSensitivity;
            _cameraPitch = Mathf.Clamp(_cameraPitch, lookUpMinMax.x, lookUpMinMax.y);

            camera.transform.localEulerAngles = new Vector3(_cameraPitch, 0, 0);
        }

        public void ToggleCrouch() {
            if (_isCrouching) {
                // Проверяем, можно ли встать
                if (CanStand()) 
                    _isCrouching = false;
            }
            else {
                _isCrouching = true;
            }
        }


        public void Jump() {
            _isJumpRequested = true;
        }

        private void HandleMovement() {
            Vector3 finalMove = _moveVector + Vector3.up * _verticalVelocity;
            characterController.Move(finalMove * Time.fixedDeltaTime);

            _isGrounded = characterController.isGrounded;
        }

        private void HandleGravityAndJump() {
            if (_isJumpRequested && _isGrounded)
                _verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * GRAVITY);

            _isJumpRequested = false;

            // Если на земле и падаем — прилипание
            if (_isGrounded && _verticalVelocity < 0)
                _verticalVelocity = -2f;

            _verticalVelocity += GRAVITY * Time.fixedDeltaTime;

            if (_verticalVelocity < -20f)
                _verticalVelocity = -20f;
        }


        private bool CanStand() {
            float headCheckDistance = standHeight - characterController.height;
            Vector3 start = transform.position + Vector3.up * characterController.height;
            return !Physics.SphereCast(start, characterController.radius, Vector3.up, out _, headCheckDistance);
        }

        private void HandleCrouch() {
            float targetHeight = _isCrouching ? crouchHeight : standHeight;
            characterController.height = Mathf.Lerp(characterController.height, targetHeight, Time.fixedDeltaTime * crouchSpeed);

            float centerY = characterController.height / 2f;
            characterController.center = new Vector3(0, centerY, 0);

            // Плавное смещение камеры
            float cameraTargetY = characterController.height - crouchCameraOffset;
            Vector3 camPos = camera.transform.localPosition;
            camPos.y = Mathf.Lerp(camPos.y, cameraTargetY, Time.fixedDeltaTime * crouchSpeed);
            camera.transform.localPosition = camPos;
        }
    }
}