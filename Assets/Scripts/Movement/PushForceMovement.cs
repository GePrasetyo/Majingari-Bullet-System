using UnityEngine;

namespace BulletSystem.Movement {
    public class PushForceMovement : BaseActorMovement {
        [SerializeField] private Rigidbody rigidbody;
        [SerializeField] private Vector3 rotationDirection = new Vector3(1, 1, 0);

        [SerializeField] private float forwardForceMagnitude;
        [SerializeField] private float torqueMagnitude;
        public override void Move(Vector2 inputAxis, out Vector2 move) {
            if (rigidbody == null) {
                move = Vector2.zero;
                return;
            }

            Vector3 appliedForce = new Vector3(inputAxis.x, inputAxis.y, 0) * forwardForceMagnitude;
            rigidbody.AddForce(appliedForce * forwardForceMagnitude, ForceMode.Impulse);
            move = new Vector2(appliedForce.x, appliedForce.y);

            float torque = Random.Range(-torqueMagnitude, torqueMagnitude);
            rigidbody.AddTorque(rotationDirection * torque, ForceMode.Impulse);
        }
    }
}