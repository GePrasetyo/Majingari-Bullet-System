using UnityEngine;

namespace BulletSystem.Movement {
    public class PostMoveRotate : PostMovementAdjuster {
        [SerializeField] private Transform transform;
        [SerializeField] private Vector3 rotationDirection = new Vector3(1, 1, 0);
        [SerializeField] private float rotationSpeed = 100f;

        public override void PostMovement() {
            transform.rotation *= Quaternion.AngleAxis(rotationSpeed * Time.deltaTime, rotationDirection);
        }
    }
}