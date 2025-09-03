using R3;
using UnityEngine;

namespace Scenes {
    public interface IInteractable {
        void Interact();
    }


    public class Character : MonoBehaviour {
        [SerializeField] private Mover _mover;
        [SerializeField] private Interactor _interactor;

        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        public void SetInput(IInput input) {
            input.moveInput
                .Subscribe(delta => _mover.Move(delta))
                .AddTo(_disposables);

            input.lookInput
                .Subscribe(delta => _mover.Look(delta))
                .AddTo(_disposables);

            input.jumpInput
                .Subscribe(_ => _mover.Jump())
                .AddTo(_disposables);

            input.crouchInput
                .Subscribe(_ => _mover.ToggleCrouch())
                .AddTo(_disposables);

            input.interactionInput
                .Subscribe(_ => _interactor.Interact())
                .AddTo(_disposables);
        }


        private void OnDestroy() {
            _disposables.Dispose();
        }
    }
}