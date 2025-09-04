using R3;
using UnityEngine;

namespace Scenes {
    public class Door : MonoBehaviour, IInteractable {
        [SerializeField] private BaseDoorView _baseDoorView;
        [SerializeField] private BaseLock _lock;
        [SerializeField] private string _closedMessage;
        [SerializeField] private string _unlockedMessage;

        private bool _isOpened;
        private bool isUnlocked => _lock == null || _lock.isUnlocked;

        public void Interact(InteractionContext context) {
            if (!isUnlocked) {
                var unlockResult = _lock.TryUnlock(context);

                Debug.Log(unlockResult ? _unlockedMessage : _closedMessage);
                return;
            }

            if (_isOpened) {
                _baseDoorView.Close();
                _isOpened = false;
            }
            else {
                _baseDoorView.Open();
                _isOpened = true;
            }
        }
    }
}