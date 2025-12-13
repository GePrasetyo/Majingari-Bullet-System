using UnityEngine;

namespace ShootingGames.Gun {
    public class BasicGun : BaseGun {
        public override int TotalAmmo => 1;
        public override int GetAmmo => 1;

        public BasicGun(GunFireSystem system, Transform gunPoint) : base(system, gunPoint) { }

        public override bool IsAmmoAvailable() {
            return true;
        }

        public override void Reload(int ammo) { }
    }
}