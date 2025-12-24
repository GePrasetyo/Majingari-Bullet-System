using BulletSystem.UI;
using Majinfwork.Pool;
using Majinfwork.StateGraph;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BulletSystem.GameState {
    /// <summary>
    /// Result Screen Loop
    /// </summary>
    public class ResultLoop : StateNodeAsset {
        [Header("UI")]
        public ResultHUD prefabResultHud;
        private ResultHUD resultHud;

        [Header("Input")]
        [SerializeField] private InputActionProperty inputActionStart;
        [SerializeField] private InputActionProperty inputActionEsc;

        public override void Begin() {
            //if (prefabResultHud.InstantiatePoolRef(out resultHud)) {
            //    GameStats lastStats = GameInstance.Instance<SpaceShipGame>().Stats.Clone();
            //    resultHud.Setup(lastStats);
            //    resultHud.gameObject.SetActive(true);
            //}
        }

        public override void Tick() {
            //if (inputActionStart.action.triggered) {
            //    request = StateRequest.Complete;
            //}

            //if (inputActionEsc.action.triggered) {
            //    request = StateRequest.Cancel;
            //}
        }

        public override void End() {

            resultHud.ReleasePoolRef<ResultHUD>(prefabResultHud);
            resultHud = null;
        }
    }
}