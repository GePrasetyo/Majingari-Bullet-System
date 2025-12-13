using UnityEngine;

namespace ShootingGames {
    public class PlayerController : MonoBehaviour {
        [field: SerializeField] public PlayerInput Input { get; private set; }
    }
}