using UnityEngine;

namespace BulletSystem.GameState {
    public abstract class StateLoop : MonoBehaviour {
        public StateRequest request { get; protected set; } = StateRequest.Sleep;

        public virtual void StartState() { request = StateRequest.Running; }
        public virtual void StopState() { request = StateRequest.Sleep; }

        public abstract void Running();
    }

    public enum StateRequest {Running, Complete, Cancel, Sleep};
}