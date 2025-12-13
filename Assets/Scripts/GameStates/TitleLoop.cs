using Majingari.Framework.Pool;
using BulletSystem.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BulletSystem.GameState {
    /// <summary>
    /// Title Screen Loop
    /// </summary>
    public class TitleLoop : StateLoop {
        [Header("UI")]
        public TitleHUD prefabTitleHud;
        private TitleHUD titleHud;

        [Header("Input")]
        [SerializeField] private InputActionReference inputActionStart;

        public override void StartState() {
            base.StartState();

            if (prefabTitleHud.InstantiatePoolRef(out titleHud)) {
                GameStats lastStats = GameInstance.Instance.Stats.Clone();
                titleHud.Setup(lastStats);
                titleHud.gameObject.SetActive(true);
            }
        }

        public override void Running() {
            if (inputActionStart.action.triggered) {
                request = StateRequest.Complete;
            }
        }

        public override void StopState() {
            base.StopState();

            titleHud.Release<TitleHUD>(prefabTitleHud);
            titleHud = null;
        }
    }
}