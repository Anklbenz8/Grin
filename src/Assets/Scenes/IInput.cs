using R3;
using UnityEngine;

namespace Scenes {
    public interface IInput {
        Observable<Vector2> moveInput { get; }
        Observable<Vector2> lookInput { get; }

        Observable<Unit> jumpInput { get; }
        Observable<Unit> crouchInput { get; }

        //например E
        Observable<Unit> interactionInput { get; }

        //например MouseClick
        Observable<Unit> actionInput { get; }
    }
}