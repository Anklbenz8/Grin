using R3;
using Scenes;
using UnityEngine;

public class Character : MonoBehaviour
{
   [SerializeField] private Mover mover;
   
   private CompositeDisposable disposables = new CompositeDisposable();
   
   public void SetInput(IInput input) {
      input.moveInput
         .Subscribe(delta => mover.Move(delta))
         .AddTo(disposables);
      
      input.lookInput
         .Subscribe(delta => mover.Look(delta))
         .AddTo(disposables);
      
      input.jumpInput
         .Subscribe(delta => mover.Jump())
         .AddTo(disposables);
   }
}
