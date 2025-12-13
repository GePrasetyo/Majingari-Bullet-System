using BulletSystem.GameState;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace BulletSystem {
    public class GameInstance : MonoBehaviour {
        public static GameInstance Instance { get; private set; }
        public GameStats Stats { get; private set; } = new GameStats();

        [SerializeField] private PlayerController playerControllerPrefab;
        public PlayerController PlayerController { get; private set; }

        [SerializeField] private GameStateMachine gameStateMachinePrefab;
        public GameStateMachine GameStateMachine { get; private set; }

        private List<ITickObject> tickCollection = new ();
        private List<IFixedTickObject> fixedTickCollection = new();

        public async void InitializeGame() {
            GameStateMachine = Instantiate(gameStateMachinePrefab);
            DontDestroyOnLoad(GameStateMachine.gameObject);
            GameStateMachine.Initialize();

            await Task.Yield();

            PlayerController = Instantiate(playerControllerPrefab);
            DontDestroyOnLoad(PlayerController.gameObject);

            await Task.Yield();

            GameStateMachine.StartGame();
        }

        public void RegisterTick(ITickObject objectTick) {
            if (!tickCollection.Contains(objectTick)) {
                tickCollection.Add(objectTick);
            }
        }

        public void RegisterTick(IFixedTickObject objectFixedTick) {
            if (!fixedTickCollection.Contains(objectFixedTick)) {
                fixedTickCollection.Add(objectFixedTick);
            }
        }

        public void UnRegisterTick(ITickObject objectTick) {
            if (tickCollection.Contains(objectTick)) {
                tickCollection.Remove(objectTick);
            }
        }

        public void UnRegisterTick(IFixedTickObject objectFixedTick) {
            if (fixedTickCollection.Contains(objectFixedTick)) {
                fixedTickCollection.Remove(objectFixedTick);
            }
        }

        private void Update() {
            for (int i = tickCollection.Count - 1; i >= 0; i--) {
                if (tickCollection[i] == null) {
                    tickCollection.RemoveAt(i);
                    continue;
                }

                tickCollection[i].Tick();
            }
        }

        private void FixedUpdate() {
            for (int i = fixedTickCollection.Count - 1; i >= 0; i--) {
                if (fixedTickCollection[i] == null) {
                    fixedTickCollection.RemoveAt(i);
                    continue;
                }

                fixedTickCollection[i].FixedTick();
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void InitGameInstance() {
            var instancePrefab = Resources.Load<GameInstance>(nameof(GameInstance));
            if (instancePrefab == null) {
                throw new System.ArgumentException("Game Instance Prefab not found in Resources Folder");
            }

            var gi = Instantiate(instancePrefab);
            gi.name = "[Service] Game Instance";
            DontDestroyOnLoad(gi.gameObject);
            Instance = gi;
            Instance.InitializeGame();
        }
    }
}