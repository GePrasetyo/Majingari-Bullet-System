using System.Collections.Generic;

namespace ShootingGames.Gun {
    public class SingleFireSystem<T> : GunFireSystem where T : Fire {
        private List<T> projectilesData = new List<T>();
        private Stack<T> poolProjectile = new Stack<T>();

        private IFireFactory<T> gunFireFactory;

        public SingleFireSystem(IFireFactory<T> gunFireFactory) {
            this.gunFireFactory = gunFireFactory;
        }

        public override void Tick() {
            for (int i = projectilesData.Count - 1; i >= 0; i--) {
                if (projectilesData[i].removeNextTick) {
                    ReleaseProjectile(i);
                }
                else {
                    projectilesData[i].Update();
                }
            }
        }

        public override void Disable() {
            base.Disable();
            for (int i = projectilesData.Count - 1; i >= 0; i--) {
                ReleaseProjectile(i);
            }

            poolProjectile.Clear();
        }

        public override void Fire(ProjectileData projectileData) {
            Fire projectile;
            if (poolProjectile.Count == 0) {
                projectile = gunFireFactory.CreateProjectile(projectileData, gunFireFactory.BulletLifeTime);
            }
            else {
                projectile = poolProjectile.Pop();
                projectile.Reuse(projectileData, gunFireFactory.BulletSpeed, gunFireFactory.BulletLifeTime);
            }

            projectile.Start();
            projectilesData.Add((T)projectile);
        }

        private void ReleaseProjectile(int index) {
            poolProjectile.Push(projectilesData[index]);
            projectilesData[index].Release();
            projectilesData.RemoveAt(index);
        }
    }
}