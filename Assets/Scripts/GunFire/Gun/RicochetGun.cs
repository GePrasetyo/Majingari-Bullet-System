using UnityEngine;

namespace ShootingGames.Gun {
    public class RicochetGun : BaseGun {
        private int totalAmmo;

        public override int TotalAmmo => totalAmmo;
        public override int GetAmmo => totalAmmo;

        public RicochetGun(GunFireSystem system, Transform gunPoint, int totalAmmo) : base(system, gunPoint) {
            this.totalAmmo = totalAmmo;
        }

        public override bool IsAmmoAvailable() {
            return true;
        }

        public override void Reload(int ammo) { }
    }
}