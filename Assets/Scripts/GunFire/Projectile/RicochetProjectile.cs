using Majingari.Framework.Pool;
using Majingari.Framework;
using UnityEngine;

namespace BulletSystem.Gun {
    public class RicochetProjectile : Fire {
        private TrailRenderer prefabKey;

        private TrailRenderer projectile;
        private Transform projectileTransform;

        private float timeRunning;
        private float moveStartTime;

        private float journeyLength;

        private int maxRicochets;
        private int ricochetsLeft;

        public RicochetProjectile(TrailRenderer prefabKey, ProjectileData projectileData, float maxSpeed, float lifeTime, int maxRicochets)
            : base(projectileData, maxSpeed, lifeTime) {
            this.prefabKey = prefabKey;
            this.maxRicochets = maxRicochets;
        }

        internal override void Start() {
            timeRunning = 0;
            ricochetsLeft = maxRicochets;
            SetStartPoint(startPosition, direction);

            prefabKey.InstantiatePoolRef(out projectile);
            projectile.enabled = false;
            projectileTransform = projectile.transform;
            projectileTransform.SetPositionAndRotation(startPosition, Quaternion.LookRotation(direction.normalized));
            projectile.enabled = true;
        }

        private void SetStartPoint(Vector3 point, Vector3 dir) {
            moveStartTime = timeRunning;
            startPosition = point;
            direction = dir.ExcludingZ().normalized;

            float remainingLifeTime = lifeTime - timeRunning;

            endPosition = startPosition + direction * maxSpeed * remainingLifeTime;
            journeyLength = maxSpeed * remainingLifeTime;
        }

        internal override void Update() {
            if (projectileTransform == null) {
                removeNextTick = true;
                return;
            }

            timeRunning += Time.deltaTime;
            float timeElapsed = timeRunning - moveStartTime;
            float distCovered = timeElapsed * currentSpeed;
            float fractionOfJourney = journeyLength > 0 ? (distCovered / journeyLength) : 1f;

            Vector3 nextLocation = Vector3.Lerp(startPosition, endPosition, fractionOfJourney);
            float lengthToTheNextLocation = (nextLocation - projectileTransform.position).magnitude;

            if (Physics.Raycast(projectileTransform.position, direction, out RaycastHit hit, lengthToTheNextLocation, collisionLayers)) {

                if (hit.collider.TryGetComponent(out Enemy enemy)) {
                    OnHit(hit);
                }

                if (ricochetsLeft > 0) {
                    ricochetsLeft--;

                    Vector3 newDirection = Vector3.Reflect(direction, hit.normal);
                    SetStartPoint(hit.point, newDirection);

                    projectileTransform.position = hit.point;
                    projectileTransform.rotation = Quaternion.LookRotation(newDirection);
                }
                else {
                    projectileTransform.position = hit.point;
                    removeNextTick = true;
                }
            }
            else {
                projectileTransform.position = nextLocation;

                if (fractionOfJourney >= 1f) {
                    removeNextTick = true;
                }
            }
        }

        internal override void Release() {
            if (projectile != null) {
                projectile.Release<TrailRenderer>(prefabKey);
                projectile = null;
                projectileTransform = null;
            }
        }
    }
}