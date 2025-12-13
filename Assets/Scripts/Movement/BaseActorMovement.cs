using System;
using UnityEngine;

namespace ShootingGames.Movement {
    [Serializable]
    public abstract class BaseActorMovement {
        public abstract void Move(Vector2 inputAxis, out Vector2 move);
    }
}