using System;
using UnityEngine.InputSystem;

namespace ShootingGames.Gun {
    public abstract class InputFireSystem : IDisposable {
        protected InputActionReference playerInput { get; init; }
        public abstract void InputTick(BaseGun gun, out InputFireStatus fireStatus);
        public abstract void Dispose();
    }

    public struct InputFireStatus {
        public float? reloading;
        public float? charging;
        public ushort? ammo;

        public bool fire;
    }
}