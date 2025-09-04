using DG.Tweening;
using UnityEngine;

namespace Scenes {
    public class DoorView : BaseDoorView {
        [SerializeField] private Transform _door;
        [SerializeField] private float _openAngle = 116f;
        [SerializeField] private float _openTime = .3f;

        private Quaternion _openRot;

        private Sequence _sequence;
        private bool _isOpen;

        public override bool isOpen => _isOpen;

        private void Awake() {
            _openRot = Quaternion.Euler(0, _openAngle, 0);
            _sequence = DOTween.Sequence()
                .Join(_door.DOLocalRotateQuaternion(_openRot, _openTime))
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