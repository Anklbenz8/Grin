using UnityEngine;

namespace Scenes {
    public abstract class BaseDoorView : MonoBehaviour {
        public abstract bool isOpen { get; }
        public abstract void Open();
        public abstract void Close();
    }
}