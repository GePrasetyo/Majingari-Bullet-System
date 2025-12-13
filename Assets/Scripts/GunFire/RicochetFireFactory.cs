using UnityEngine;

namespace BulletSystem.Gun {
    [CreateAssetMenu(fileName = "Ricochet Fire Factory", menuName = "Shooting Games Scriptable/Fire/Ricochet Fire Factory")]
    public class RicochetFireFactory : GunFireFactory, IFireFactory<RicochetProjectile> {
        [SerializeField] private TrailRenderer visualProjectilePrefab;
        [field: SerializeField] public float chargeTime { get; private set; } = 6;
        [field: SerializeField] public float timeBetweenBullet { get; private set; } = 0.2f;

        [field: SerializeField] public int totalBullets { get; private set; } = 5;
        [field: SerializeField] public int maxricochets { get; private set; } = 6;

        public float BulletSpeed => base.bulletSpeed;
        public float BulletLifeTime => base.bulletLifeTime;

        public RicochetProjectile CreateProjectile(ProjectileData rawData, float lifeTime) {
            return new RicochetProjectile(visualProjectilePrefab, rawData, bulletSpeed, lifeTime, maxricochets);
        }

        public override BaseGun CreateGun(Transform gunPoint) {
            return new RicochetGun(CreateSystem(), gunPoint, totalBullets);
        }

        public override GunFireSystem CreateSystem() {
            if (this.TryResolveSystem(out GunFireSystem system)) {
                system.Initialize();
                return system;
            }

            system = new RicochetFireSystem<RicochetProjectile>(this);
            system.Initialize();

            GunFireSystemCollection.RegisterSystem(this, system);
            return system;
        }

        internal override InputFireSystem CreateInputSystem() {
            return new InputChargeFireSystem(playerInput, new InputChargeParam { 
                chargeTime = chargeTime,
                timeBetweenBullet = timeBetweenBullet
            });
        }
    }
}