using System;
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
        [SerializeField] private float standMoveSpeed = 3f;
        [SerializeField] private float crouchSpeedMultiplier = 0.5f;

        [SerializeField] private float lookSensitivity = 3f;
        [SerializeField] private float crouchCameraOffset = 0.5f;

        [SerializeField] private Vector2 lookUpMinMax = new Vector2(-80f, 80f);

        private Vector3 _moveVector;
        private float _cameraPitch = 0f;
        private float _verticalVelocity;
        private bool _isGrounded;
        private bool _isCrouching;
        private bool _isJumpRequested;
        private bool _isSmoothCrouchRequested;
        private float _targetHeight;
        private float _targetCameraY;
        private float _characterCenterY;
        private float _moveSpeed;

        private void Awake() {
            _moveSpeed = standMoveSpeed;
        }

        private void FixedUpdate() {
            HandleGravityAndJump();
            HandleMovement();
            if (_isSmoothCrouchRequested)
                HandleCrouch();
        }

        public void Move(Vector2 inputDirection) {
            Vector3 forward = transform.forward;
            Vector3 right = transform.right;
            // Нормализуем input, чтобы диагональная скорость не превышала скорость вперед/назад
            Vector2 inputNormalized = Vector2.ClampMagnitude(inputDirection, 1f);

            _moveVector = (forward * inputNormalized.y + right * inputNormalized.x) * _moveSpeed;
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
                    StandUp();
            }
            else {
                Crouch();
            }
        }


        public void Jump() {
            // Если присели - встаем перед прыжком
            if (_isCrouching && CanStand()) {
                StandUp();
                return;
            }

            if (!_isCrouching)
                _isJumpRequested = true;
        }

        private void Crouch() {
            _isSmoothCrouchRequested = true;

            _isCrouching = true;
            _targetHeight = crouchHeight;
            _characterCenterY = crouchHeight / 2f;
            _targetCameraY = crouchHeight - crouchCameraOffset;
            _moveSpeed = standMoveSpeed * crouchSpeedMultiplier;
        }

        private void StandUp() {
            _isSmoothCrouchRequested = true;

            _isCrouching = false;
            _targetHeight = standHeight;

            _characterCenterY = standHeight / 2f;
            _targetCameraY = standHeight - crouchCameraOffset;
            _moveSpeed = standMoveSpeed;
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
            characterController.height = Mathf.Lerp(characterController.height, _targetHeight, Time.fixedDeltaTime * crouchSpeed);

            //Центр Character Controller
            float currentCenterY = characterController.center.y;
            float newCenterY = Mathf.Lerp(currentCenterY, _characterCenterY, Time.fixedDeltaTime * crouchSpeed);
            characterController.center = new Vector3(0, newCenterY, 0);

            // Плавное смещение камеры
            Vector3 camPos = camera.transform.localPosition;
            camPos.y = Mathf.Lerp(camPos.y, _targetCameraY, Time.fixedDeltaTime * crouchSpeed);
            camera.transform.localPosition = camPos;

            if (Mathf.Abs(characterController.height - _targetHeight) > 0.01f)
                return;

            characterController.height = _targetHeight;
            characterController.center = new Vector3(0, _characterCenterY, 0);
            camPos.y = _targetCameraY;
            camera.transform.localPosition = camPos;

            _isSmoothCrouchRequested = false;
        }
    }
}