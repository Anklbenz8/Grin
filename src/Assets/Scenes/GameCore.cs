
using Scenes;
using UnityEngine;

public class GameCore : MonoBehaviour {
    [SerializeField] private Character character;
    private MouseKeyboardInput input;
    
    
    private void Start() {
        input = new MouseKeyboardInput();
        input.Start();
        character.SetInput(input);
    }
}