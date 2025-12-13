using System;
using UnityEngine;
using ShootingGames.Utils;

namespace ShootingGames.Movement {
    [Serializable]
    public abstract class PostMovementAdjuster {
        public abstract void PostMovement();
    }

    [Serializable]
    public class PostMovement {
        [ClassReference, SerializeReference] public PostMovementAdjuster mover;
    }
}