using System;
using System.Collections.Generic;
using UnityEngine;

namespace BulletSystem.GameState {
    public class GameStateMachine : MonoBehaviour {
        [SerializeField] private State[] states;
        private Dictionary<StateLoop, State> stateMap = new Dictionary<StateLoop, State>();
        private StateLoop currentState;

        public void Initialize() {
            for (int i = 0; i < states.Length; i++) {
                stateMap[states[i].keyState] = states[i];
            }
        }

        public void StartGame() {
            StateLoop state = states[0].keyState;
            state.StartState();

            currentState = state;
        }

        private void Update() {
            if (currentState != null) {
                currentState.Running();

                if (currentState.request == StateRequest.Complete) {
                    currentState.StopState();
                    GoToNextState();
                }

                if (currentState.request == StateRequest.Cancel) {
                    currentState.StopState();
                    GoToPreviousState();
                }
            }
        }

        private void GoToNextState() {
            if(stateMap.TryGetValue(currentState, out State stateConnection)){
                stateConnection.nextState.StartState();
                currentState = stateConnection.nextState;
            }
            else {
                Debug.LogError("No Next State Found on The Map");
                StartGame();
            }
        }

        private void GoToPreviousState() {
            if (stateMap.TryGetValue(currentState, out State stateConnection)) {
                stateConnection.prevState.StartState();
                currentState = stateConnection.prevState;
            }
            else {
                Debug.LogError("No Next State Found on The Map");
                StartGame();
            }
        }
    }

    [Serializable]
    public struct State {
        public StateLoop keyState;
        public StateLoop nextState;
        public StateLoop prevState;
    }
}