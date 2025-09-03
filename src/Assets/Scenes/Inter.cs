using Scenes;
using UnityEngine;

public class Inter : MonoBehaviour, IInteractable {
    public void Interact() {
        Debug.Log(gameObject.name);
    }
}