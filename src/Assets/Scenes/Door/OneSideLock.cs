using UnityEngine;

namespace Scenes {
    public class OneSideLock : BaseLock {
        [SerializeField] private bool _isFrontLocked;

        public override bool isUnlocked => _isUnlocked;

        private bool _isUnlocked;

        public override bool TryUnlock(InteractionContext context) {
            float dot = Vector3.Dot(transform.forward, context.forward);
            bool inFront = dot < 0;

            if (_isFrontLocked == inFront) {
                _isUnlocked = true;
                return true;
            }

            return false;
        }
    }
}