using System;
using UnityEngine;
using Majinfwork;

namespace BulletSystem.Movement {
    [Serializable]
    public abstract class PostMovementAdjuster {
        public abstract void PostMovement();
    }

    [Serializable]
    public class PostMovement {
        [ClassReference, SerializeReference] public PostMovementAdjuster mover;
    }
}