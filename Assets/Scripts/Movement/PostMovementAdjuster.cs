using System;
using UnityEngine;
using Majingari.Framework;

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