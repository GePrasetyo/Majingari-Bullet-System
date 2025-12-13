using System;
using UnityEngine;

namespace BulletSystem.Gun {
    public abstract class BaseGun : IDisposable {
        protected GunFireSystem gunFireSystem;
        protected Transform gunPoint;
        
        public abstract int TotalAmmo { get; }
        public abstract int GetAmmo { get; }

        public BaseGun(GunFireSystem system, Transform gunPoint) {
            gunFireSystem = system;
            this.gunPoint = gunPoint;
        }

        public abstract bool IsAmmoAvailable();

        public virtual void Fire(HitID hitID) {
            ProjectileData data = new ProjectileData {
                gunPointRef = gunPoint,
                startPosition = gunPoint.position,
                direction = gunPoint.forward,
                HitID = hitID
            };

            gunFireSystem.Fire(data);
        }

        public abstract void Reload(int ammo);
        
        public virtual void Dispose() {
            gunFireSystem = null;
            gunPoint = null;
        }
    }
}