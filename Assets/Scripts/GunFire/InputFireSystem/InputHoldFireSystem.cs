using UnityEngine;
using UnityEngine.InputSystem;

namespace BulletSystem.Gun {
    public class InputHoldFireSystem : InputFireSystem {
        private InputHoldParam holdData;

        private float reloadingTimer;
        private bool isReloading;

        public InputHoldFireSystem(InputActionReference playerInput, InputHoldParam param) : base() {
            this.playerInput = playerInput;
            holdData = param;

            reloadingTimer = 0;
            isReloading = false;
        }

        public override void InputTick(BaseGun gun, out InputFireStatus fireStatus) {
            bool fire = false;
            bool pressed = playerInput.action.ReadValue<float>() > 0;
            float? reloadProgress = null;
            bool fullAmmo = gun.GetAmmo == gun.TotalAmmo;

            if (gun.IsAmmoAvailable()) {
                if(pressed) {
                    fire = true;
                    isReloading = false;
                }
                else {
                    fire = false;
                    isReloading = !fullAmmo;
                }
            }
            else {
                fire = false;
                isReloading = !pressed;
            }

            if (isReloading) {
                reloadingTimer += Time.deltaTime * (1/ holdData.reloadTime);
                reloadProgress = Mathf.Min(1, reloadingTimer/ holdData.reloadTime);

                if (reloadProgress >= 1f) {
                    gun.Reload(1);
                    reloadingTimer = 0;
                }
            }
            else {
                reloadingTimer = 0;
            }

            fireStatus = new() {
                charging = null,
                reloading = reloadProgress,
                ammo = (ushort)gun.GetAmmo,
                fire = fire
            };
        }

        public override void Dispose() {
            reloadingTimer = 0;
            isReloading = false;
        }
    }

    public struct InputHoldParam {
        public float reloadTime { get; init; }
    }
}