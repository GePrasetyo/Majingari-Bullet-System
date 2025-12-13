using UnityEngine;
using UnityEngine.InputSystem;

namespace ShootingGames {
    public class PlayerInput : MonoBehaviour {
        [SerializeField] private InputActionReference inputActionMainAxis;
        [SerializeField] private InputActionReference inputActionSwitchGun;
        public bool isSwitchGunPressed => inputActionSwitchGun.action.triggered;

        [field: SerializeField] public Vector2 MainAxis { get; private set; }

        private void Update() {
            MainAxis = inputActionMainAxis.action.ReadValue<Vector2>();
        }
    }
}