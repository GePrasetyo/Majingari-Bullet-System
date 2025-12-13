using UnityEngine;

namespace BulletSystem.Gun {
    public class LaserGun : BaseGun {
        private int maxAmmo;
        private float currentAmmo;
        private float fireInterval;

        public override int TotalAmmo => maxAmmo;
        public override int GetAmmo => Mathf.RoundToInt(currentAmmo);

        public LaserGun(GunFireSystem system, Transform gunPoint, int maxAmmo, float fireInterval) : base(system, gunPoint) {
            this.maxAmmo = maxAmmo;
            this.fireInterval = fireInterval;
            currentAmmo = maxAmmo;
        }

        public override bool IsAmmoAvailable() {
            return currentAmmo > 0;
        }

        public override void Fire(HitID hitID) {
            currentAmmo = Mathf.Max(currentAmmo-Time.deltaTime * fireInterval, 0);

            ProjectileData data = new ProjectileData {
                gunPointRef = gunPoint,
                startPosition = gunPoint.position,
                direction = gunPoint.forward,
                HitID = hitID
            };

            gunFireSystem.Fire(data);
        }

        public override void Reload(int ammo) {
            currentAmmo = Mathf.Min(currentAmmo + ammo, maxAmmo);
        }
    }
}