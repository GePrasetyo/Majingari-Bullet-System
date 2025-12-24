using BulletSystem.UI;
using Majinfwork.Pool;
using Majinfwork.StateGraph;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BulletSystem.GameState {
    public class TitleScreen : StateNodeAsset {
        [Header("UI")]
        public TitleHUD prefabTitleHud;
        private TitleHUD titleHud;

        [Header("Input")]
        [SerializeField] private InputActionProperty inputActionStart;

        public StateTransition toGame;

        public override void Begin() {
            if (prefabTitleHud.InstantiatePoolRef(out titleHud)) {
                //GameStats lastStats = GameInstance.Instance<SpaceShipGame>().Stats.Clone();
                //titleHud.Setup(lastStats);
                titleHud.gameObject.SetActive(true);
            }
        }

        public override void Tick() {
            if (inputActionStart.action.triggered) {
                TriggerExit(toGame);
            }
        }

        public override void End() {
            titleHud.ReleasePoolRef<TitleHUD>(prefabTitleHud);
            titleHud = null;
        }
    }
}