using System;
using UnityEngine;

namespace ShootingGames.Movement {
    [Serializable]
    public class PhysxForceMovement : BaseActorMovement {
        [SerializeField] private Rigidbody rigidbody;
        [SerializeField] private float forwardForceMagnitude;
        [SerializeField] private float torqueMagnitude;
        [SerializeField] private float maxSpeed;

        public override void Move(Vector2 inputAxis, out Vector2 move) {
            if (rigidbody == null) {
                move = Vector2.zero;
                return;
            }

            float thrustInput = inputAxis.y;
            float rotationInput = inputAxis.x;

            Vector3 appliedForce = Vector3.zero;

            if (rotationInput != 0) {
                float torque = -rotationInput * torqueMagnitude;
                rigidbody.AddTorque(rigidbody.transform.forward * torque, ForceMode.Acceleration);
            }

            if (thrustInput != 0) {
                Vector3 forwardVector = rigidbody.transform.up;
                appliedForce = forwardVector * thrustInput * forwardForceMagnitude;
                rigidbody.AddForce(appliedForce, ForceMode.Acceleration);
            }

            if (rigidbody.linearVelocity.magnitude > maxSpeed) {
                rigidbody.linearVelocity = rigidbody.linearVelocity.normalized * maxSpeed;
            }

            move = new Vector2(appliedForce.x, appliedForce.y);
        }
    }
}