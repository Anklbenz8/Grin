using System;
using R3;
using UnityEngine;

namespace Scenes {
    public class MouseKeyboardInput : IInput, IDisposable {
        public Observable<Vector2> moveInput => _moveInputSubject;
        public Observable<Vector2> lookInput => _lookInputSubject;

        public Observable<Unit> jumpInput => _jumpInputSubject;
        public Observable<Unit> crouchInput => _crouchInputSubject;
        public Observable<Unit> interactionInput => _interactionInputSubject;
        public Observable<Unit> actionInput => _actionInputSubject;

        private readonly Subject<Vector2> _moveInputSubject = new();
        private readonly Subject<Vector2> _lookInputSubject = new();
        private readonly Subject<Unit> _jumpInputSubject = new();
        private readonly Subject<Unit> _crouchInputSubject = new();
        private readonly Subject<Unit> _actionInputSubject = new();
        private readonly Subject<Unit> _interactionInputSubject = new();


        private CompositeDisposable _subscribes;

        public void Start() {
            _subscribes = new();
            Observable.EveryUpdate()
                .Select(_ => new Vector2(Input.GetAxis("Mouse X"), Input.GetAxisRaw("Mouse Y")))
                .Where(delta => delta != Vector2.zero)
                .Subscribe(delta => _lookInputSubject.OnNext(delta))
                .AddTo(_subscribes);

            Observable.EveryUpdate()
                .Select(_ => new Vector2(Input.GetAxis("Horizontal"), Input.GetAxisRaw("Vertical")))
                //  .DistinctUntilChanged()
                .Subscribe(delta => _moveInputSubject.OnNext(delta))
                .AddTo(_subscribes);

            Observable.EveryUpdate()
                .Select(_ => Input.GetMouseButtonDown(0))
                .Where(action => action)
                .Subscribe(_ => _actionInputSubject.OnNext(Unit.Default))
                .AddTo(_subscribes);

            Observable.EveryUpdate()
                .Select(_ => Input.GetKey(KeyCode.E))
                .Where(action => action)
                .Subscribe(_ => _interactionInputSubject.OnNext(Unit.Default))
                .AddTo(_subscribes);

            Observable.EveryUpdate()
                .Select(_ => Input.GetKey(KeyCode.Space))
                .Where(action => action)
                .Subscribe(_ => _jumpInputSubject.OnNext(Unit.Default))
                .AddTo(_subscribes);

            Observable.EveryUpdate()
                .Select(_ => Input.GetKeyDown(KeyCode.LeftControl))
                .Where(action => action)
                .Subscribe(_ => _crouchInputSubject.OnNext(Unit.Default))
                .AddTo(_subscribes);
        }

        public void Dispose() {
            _subscribes?.Dispose();
            _moveInputSubject.Dispose();
            _lookInputSubject.Dispose();
        }
    }
}