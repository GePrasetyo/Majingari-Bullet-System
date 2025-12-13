using UnityEngine;
using UnityEngine.InputSystem;

namespace BulletSystem.Gun {
    public abstract class GunFireFactory : ScriptableObject {
        [SerializeField] protected InputActionReference playerInput;
        [field: SerializeField] protected float bulletLifeTime = 4f;
        [field: SerializeField] protected float bulletSpeed = 8f;

        public abstract BaseGun CreateGun(Transform gunPoint);
        public abstract GunFireSystem CreateSystem();
        internal abstract InputFireSystem CreateInputSystem();
    }
}