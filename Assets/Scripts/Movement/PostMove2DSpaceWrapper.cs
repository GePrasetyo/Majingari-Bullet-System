using Majingari.Framework;
using UnityEngine;

namespace BulletSystem.Movement {
    public class PostMove2DSpaceWrapper : PostMovementAdjuster {
        [SerializeField] private Transform transform;
        [SerializeField] private bool position = true;
        [SerializeField] private bool rotation = true;

        public override void PostMovement() {
            if (position) {
                transform.position = transform.position.ExcludingZ();
            }

            if (rotation) {
                transform.rotation = transform.rotation.IsolateZRotation();
            }
        }
    }
}