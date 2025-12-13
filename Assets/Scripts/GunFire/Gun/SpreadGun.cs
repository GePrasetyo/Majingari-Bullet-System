using UnityEngine;

namespace ShootingGames.Gun {
    public class SpreadGun : BaseGun {
        private int totalAmmo;
        private float totalSpreadAngle;

        public override int TotalAmmo => totalAmmo;
        public override int GetAmmo => totalAmmo;

        public SpreadGun(GunFireSystem system, Transform gunPoint, int totalAmmo, float totalSpreadAngle) : base(system, gunPoint) {
            this.totalAmmo = totalAmmo;
            this.totalSpreadAngle = totalSpreadAngle;
        }

        public override bool IsAmmoAvailable() {
            return true;
        }

        public override void Fire(HitID hitID) {
            float startAngle = -totalSpreadAngle / 2f;
            float angleStep = totalSpreadAngle / (totalAmmo - 1);

            if (totalAmmo == 1) {
                angleStep = 0f;
                startAngle = 0f;
            }

            for (int i = 0; i < totalAmmo; i++) {
                float currentAngle = startAngle + i * angleStep;
                Quaternion rotation = Quaternion.AngleAxis(currentAngle, Vector3.forward);
                Vector3 newDirection = rotation * gunPoint.forward;

                ProjectileData projectileData = new ProjectileData {
                    startPosition = gunPoint.position,
                    direction = newDirection,
                    HitID = hitID
                };
                
                gunFireSystem.Fire(projectileData);

            }
        }

        public override void Reload(int ammo) { }
    }
}