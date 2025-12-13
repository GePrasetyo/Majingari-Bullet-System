using UnityEngine;
using UnityEngine.InputSystem;

namespace ShootingGames.Gun {
    public class InputChargeFireSystem : InputFireSystem {
        private InputChargeParam chargeData;
        private float chargingTime;
        private float delayFireTime;

        private int bulletToShot;
        private bool isCharging;

        public InputChargeFireSystem(InputActionReference playerInput, InputChargeParam param) : base() {
            this.playerInput = playerInput;
            chargeData = param;

            isCharging = false;
            bulletToShot = 0;
            chargingTime = 0;
            delayFireTime = 0;
        }

        public override void InputTick(BaseGun gun, out InputFireStatus fireStatus) {
            float? chargingProgress = 0;
            bool fire = false;

            if (bulletToShot == 0) {
                Charge(gun, out chargingProgress);
            }
            else {
                if (delayFireTime <= 0) {
                    delayFireTime = chargeData.timeBetweenBullet;
                    bulletToShot = Mathf.Max(bulletToShot - 1, 0);
                    fire = true;
                }
                else {
                    fire = false;
                    delayFireTime -= Time.deltaTime;
                }
            }

            fireStatus = new() {
                charging = chargingProgress,
                reloading = null,
                ammo = null,
                fire = fire
            };
        }

        private void Charge(BaseGun gun, out float? chargingProgress) {
            bool charging = playerInput.action.ReadValue<float>() > 0;
            float fullCharginTime = chargeData.chargeTime * (gun.GetAmmo / gun.TotalAmmo);

            if (charging) {
                if (!isCharging) {
                    chargingTime = 0;
                }
                else {
                    chargingTime += Time.deltaTime;
                }

                isCharging = true;
                chargingProgress = Mathf.Min(chargingTime / fullCharginTime, 1f);
            }
            else {
                if (isCharging) {
                    bulletToShot = Mathf.FloorToInt(gun.GetAmmo * Mathf.Min(chargingTime / fullCharginTime, 1f));
                }

                chargingProgress = null;
                isCharging = false;
            }
        }

        public override void Dispose() {
            isCharging = false;
            bulletToShot = 0;
            chargingTime = 0;
            delayFireTime = 0;
        }
    }

    public struct InputChargeParam {
        public float timeBetweenBullet { get; init; }
        public float chargeTime { get; init; }
    }
}