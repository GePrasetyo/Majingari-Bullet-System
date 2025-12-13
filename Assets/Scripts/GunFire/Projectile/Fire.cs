using Majingari.Framework;
using UnityEngine;

namespace BulletSystem.Gun {
    public abstract class Fire {
        public const int collisionLayers = 1 << 0 | 1 << 3;

        public HitID HitID { get; protected set; }
        protected Vector3 startPosition;
        protected Vector3 direction;

        protected float lifeTime;

        protected float currentSpeed;
        protected float maxSpeed;

        protected Vector3 endPosition;
        public bool removeNextTick { get; protected set; }

        internal Fire(ProjectileData projectileData, float maxSpeed, float lifeTime) {
            Initialize(projectileData, maxSpeed, lifeTime);
        }

        internal void Reuse(ProjectileData projectileData, float maxSpeed, float lifeTime) {
            Initialize(projectileData, maxSpeed, lifeTime);
        }

        protected void Initialize(ProjectileData projectileData, float maxSpeed, float lifeTime) {
            this.startPosition = projectileData.startPosition;
            this.direction = projectileData.direction.ExcludingZ();
            this.HitID = projectileData.HitID;

            this.lifeTime = Mathf.Max(0.1f, lifeTime);
            currentSpeed = maxSpeed;
            this.maxSpeed = maxSpeed;

            removeNextTick = false;
        }

        protected void OnHit(RaycastHit hit) {
            if (hit.collider.TryGetComponent(out IHitable hitTarget)) {
                if (hitTarget.IsHitValid(HitID)){
                    hitTarget.OnHit();
                }
            }
        }

        internal abstract void Start();
        internal abstract void Update();
        internal abstract void Release();
    }

    public struct ProjectileData {
        public Transform gunPointRef;
        public Vector3 startPosition;
        public Vector3 direction;
        public HitID HitID { get; init; }
    }

    public interface IFireFactory<TFire> where TFire : Fire {
        public float BulletLifeTime { get; }
        public float BulletSpeed { get; }
        public TFire CreateProjectile(ProjectileData rawData, float lifeTime);
    }
}