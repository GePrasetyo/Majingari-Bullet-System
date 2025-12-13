using UnityEngine;

namespace BulletSystem {
    public class PlayerController : MonoBehaviour {
        [field: SerializeField] public PlayerInput Input { get; private set; }
    }
}