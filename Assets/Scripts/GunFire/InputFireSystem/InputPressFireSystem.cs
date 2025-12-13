
using UnityEngine.InputSystem;

namespace ShootingGames.Gun {
    public class InputPressFireSystem : InputFireSystem {
        public InputPressFireSystem(InputActionReference playerInput) : base() {
            this.playerInput = playerInput;
        }

        public override void InputTick(BaseGun gun, out InputFireStatus fireStatus) {
            fireStatus = new() {
                charging = null,
                reloading = null,
                ammo = null,
                fire = playerInput.action.triggered
            };
        }

        public override void Dispose() { }
    }
}