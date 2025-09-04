using Scenes;
using UnityEngine;

public class Inter : MonoBehaviour, IInteractable {
    public void Interact(InteractionContext context) {
        Debug.Log(gameObject.name);
    }
}