using System;
using UnityEngine;

namespace ShootingGames.Movement {
    [Serializable]
    public class PostMoveScreenWrapper : PostMovementAdjuster {
        [SerializeField] private Transform transform;
        [SerializeField] private float offset;

        private Camera mainCamera;

        public override void PostMovement() {
            if (mainCamera?.isActiveAndEnabled != true) {
                mainCamera = Camera.main;
            }

            float halfHeight = mainCamera.orthographicSize;
            float halfWidth = mainCamera.aspect * halfHeight;

            float worldHeight = halfHeight * 2f;
            float worldWidth = halfWidth * 2f;

            Vector3 viewPos = mainCamera.WorldToViewportPoint(transform.position);
            Vector3 newPosition = transform.position;
            float viewportOffset = offset / worldWidth;

            if (viewPos.x < -viewportOffset) {
                newPosition.x += worldWidth + offset;
            }
            else if (viewPos.x > 1 + viewportOffset) {
                newPosition.x -= worldWidth + offset;
            }

            if (viewPos.y < -viewportOffset) {
                newPosition.y += worldHeight + offset;
            }
            else if (viewPos.y > 1 + viewportOffset) {
                newPosition.y -= worldHeight + offset;
            }

            if (newPosition != transform.position) {
                transform.position = newPosition;
            }
        }
    }
}