using UnityEngine;

namespace ShootingGames.Gun {
    [CreateAssetMenu(fileName = "Basic Fire Factory", menuName = "Shooting Games Scriptable/Fire/Basic Fire Factory")]
    public class BasicFireFactory : GunFireFactory, IFireFactory<ForwardProjectile> {
        [SerializeField] private Transform visualProjectilePrefab;
        public float BulletSpeed => base.bulletSpeed;
        public float BulletLifeTime => base.bulletLifeTime;

        public ForwardProjectile CreateProjectile(ProjectileData rawData, float lifeTime) {
            return new ForwardProjectile(visualProjectilePrefab, rawData, bulletSpeed, lifeTime);
        }

        public override BaseGun CreateGun(Transform gunPoint) {
            return new BasicGun(CreateSystem(), gunPoint);
        }

        public override GunFireSystem CreateSystem() {
            if(this.TryResolveSystem(out GunFireSystem system)) {
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