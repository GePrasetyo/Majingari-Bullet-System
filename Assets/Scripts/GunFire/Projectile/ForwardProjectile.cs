using ShootingGames.Pool;
using UnityEngine;

namespace ShootingGames.Gun {
    public class ForwardProjectile : Fire {
        private Transform prefabKey;
        private Transform projectileTransform;

        private float timerRunning;
        private float journeyLength;

        public ForwardProjectile(Transform prefabKey, ProjectileData projectileData, float maxSpeed, float lifeTime)
            : base(projectileData, maxSpeed, lifeTime) {
            this.prefabKey = prefabKey;
        }

        internal override void Start() {
            timerRunning = 0;
            endPosition = startPosition + direction.normalized * maxSpeed * lifeTime;
            journeyLength = Mathf.Max(maxSpeed * lifeTime, 0.1f);

            prefabKey.InstantiatePoolRef(startPosition, Quaternion.LookRotation(direction.normalized), out projectileTransform);
        }

        internal override void Update() {
            timerRunning += Time.deltaTime;
            float distCovered = timerRunning * currentSpeed;
            float fractionOfJourney = distCovered / journeyLength;
            Vector3 nextLocation = Vector3.Lerp(startPosition, endPosition, fractionOfJourney);
            float lengthToTheNextLocation = (nextLocation - projectileTransform.position).magnitude;

            if (Physics.Raycast(projectileTransform.position, direction, out RaycastHit hit, lengthToTheNextLocation, collisionLayers)) {
                projectileTransform.position = hit.point;
                removeNextTick = true;
                OnHit(hit);
            }
            else {
                projectileTransform.position = nextLocation;

                if (fractionOfJourney >= 1f) {
                    removeNextTick = true;
                }
            }
        }

        internal override void Release() {
            projectileTransform.Release<Transform>(this.prefabKey);
            projectileTransform = null;
        }
    }
}