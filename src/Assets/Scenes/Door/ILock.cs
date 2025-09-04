using UnityEngine;

namespace Scenes {
    public abstract class BaseLock : MonoBehaviour {
        public abstract bool isUnlocked { get; }
        public abstract bool TryUnlock(InteractionContext context);
    }
}