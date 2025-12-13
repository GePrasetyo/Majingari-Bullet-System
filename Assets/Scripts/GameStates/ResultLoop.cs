using ShootingGames.Pool;
using ShootingGames.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ShootingGames.GameState {
    /// <summary>
    /// Result Screen Loop
    /// </summary>
    public class ResultLoop : StateLoop {
        [Header("UI")]
        public ResultHUD prefabResultHud;
        private ResultHUD resultHud;

        [Header("Input")]
        [SerializeField] private InputActionReference inputActionStart;
        [SerializeField] private InputActionReference inputActionEsc;

        public override void StartState() {
            base.StartState();

            if (prefabResultHud.InstantiatePoolRef(out resultHud)) {
                GameStats lastStats = GameInstance.Instance.Stats.Clone();
                resultHud.Setup(lastStats);
                resultHud.gameObject.SetActive(true);
            }
        }

        public override void Running() {
            if (inputActionStart.action.triggered) {
                request = StateRequest.Complete;
            }

            if (inputActionEsc.action.triggered) {
                request = StateRequest.Cancel;
            }
        }

        public override void StopState() {
            base.StopState();

            resultHud.Release<ResultHUD>(prefabResultHud);
            resultHud = null;
        }
    }
}