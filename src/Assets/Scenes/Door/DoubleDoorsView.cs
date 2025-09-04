using DG.Tweening;
using UnityEngine;

namespace Scenes {
    public class DoubleDoorsView : BaseDoorView {
        [SerializeField] private Transform _leftDoor;
        [SerializeField] private Transform _rightDoor;

        [SerializeField] private float _leftOpenAngle = 116f;
        [SerializeField] private float _rightOpenAngle = -130f;
        [SerializeField] private float _openTime = .3f;

        private Quaternion _leftClosedRot;
        private Quaternion _rightClosedRot;

        private Quaternion _leftOpenRot;
        private Quaternion _rightOpenRot;

        private Sequence _sequence;
        private bool _isOpen;

        public override bool isOpen => _isOpen;

        private void Awake() {
            // Запоминаем исходное положение дверей
            _leftClosedRot = _leftDoor.localRotation;
            _rightClosedRot = _rightDoor.localRotation;

            // Считаем открытые углы
            _leftOpenRot = Quaternion.Euler(0, _leftOpenAngle, 0);
            _rightOpenRot = Quaternion.Euler(0, _rightOpenAngle, 0);

            // Создаём один Sequence с AutoKill = false
            _sequence = DOTween.Sequence()
                .Join(_leftDoor.DOLocalRotateQuaternion(_leftOpenRot, _openTime))
                .Join(_rightDoor.DOLocalRotateQuaternion(_rightOpenRot, _openTime))
                .SetAutoKill(false)
                .Pause();
        }

        public override void Open() {
            if (_isOpen) return;
            _sequence.PlayForward();
            _isOpen = true;
        }

        public override void Close() {
            if (!_isOpen) return;
            _sequence.PlayBackwards();
            _isOpen = false;
        }
    }
}