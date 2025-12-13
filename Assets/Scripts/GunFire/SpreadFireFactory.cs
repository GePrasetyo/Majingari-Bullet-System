using UnityEngine;

namespace ShootingGames.Gun {
    [CreateAssetMenu(fileName = "Spread Fire Factory", menuName = "Shooting Games Scriptable/Fire/Spread Fire Factory")]
    public class SpreadFireFactory : GunFireFactory, IFireFactory<ForwardProjectile> {
        [SerializeField] private Transform visualProjectilePrefab;
        [field: SerializeField] public int TotalBullets { get; private set; } = 5;
        [field: SerializeField] public float TotalSpreadAngle { get; private set; } = 30f;

        public float BulletSpeed => base.bulletSpeed;
        public float BulletLifeTime => base.bulletLifeTime;

        public ForwardProjectile CreateProjectile(ProjectileData rawData, float lifeTime) {
            return new ForwardProjectile(visualProjectilePrefab, rawData, bulletSpeed, lifeTime);
        }

        public override BaseGun CreateGun(Transform gunPoint) {
            return new SpreadGun(CreateSystem(), gunPoint, TotalBullets, TotalSpreadAngle);
        }

        public override GunFireSystem CreateSystem() {
            if (this.TryResolveSystem(out GunFireSystem system)) {
                system.Initialize();
                return system;
            }

            system = new SingleFireSystem<ForwardProjectile>(this);
            system.Initialize();

            GunFireSystemCollection.RegisterSystem(this, system);
            return system;
        }

        internal override InputFireSystem CreateInputSystem() {
            return new InputPressFireSystem(playerInput);
        }
    }
}