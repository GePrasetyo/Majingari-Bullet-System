using BulletSystem.Gun;
using Majinfwork.Pool;
using Majinfwork.StateGraph;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BulletSystem.GameState {
    /// <summary>
    /// Stage main loop
    /// </summary>
    public class StageLoop : StateNodeAsset {
        [Header("Prefab")]
        public Player prefabPlayer;
        private Player player;
        public EnemySpawner prefabEnemySpawner;
        private EnemySpawner enemySpawner;

        //[Header("Layout")]
        //public Transform stageTransform;

        [Header("Input")]
        [SerializeField] private InputActionProperty inputActionEscape;

        [Header("Music")]
        [SerializeField] private float musicBpm;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private Interval spawningInterval;
        [SerializeField] private Interval pushSignal;
        [SerializeField] private Interval beat;

        public StateTransition backToTitle;
        public StateTransition toResult;

        public override void Begin() {
            SetupStage();
            audioSource.Play();
        }

        public override void Tick() {
            if (inputActionEscape.action.triggered) {
                TriggerExit(backToTitle);
            }

            if(player.health <= 0 || !audioSource.isPlaying) {
                TriggerExit(toResult);
            }

            spawningInterval.CheckForNewInterval(audioSource, musicBpm);
            pushSignal.CheckForNewInterval(audioSource, musicBpm);
            beat.CheckForNewInterval(audioSource, musicBpm);
        }

        public override void End() {
            CleanupStage();
            audioSource.Stop();
        }

        private void SetupStage() {
            //SpaceShipGame.Instance<SpaceShipGame>()?.Stats.ResetScore();
            //PlayerController pc = SpaceShipGame.Instance<SpaceShipGame>().PlayerController;

            //if (prefabPlayer.InstantiatePoolRef(stageTransform, out player)) {
            //    player.transform.position = new Vector3(0, -4, 0);
            //    player.Initialize(pc, pushSignal);
            //}

            //if (prefabEnemySpawner.InstantiatePoolRef(stageTransform, out enemySpawner)) {
            //    spawningInterval.trigger += enemySpawner.Spawn;
            //    enemySpawner.SetEvent(pushSignal, beat);
            //}
        }

        void CleanupStage() {
            player.ReleasePoolRef<Player>(prefabPlayer);
            player = null;

            spawningInterval.trigger -= enemySpawner.Spawn;
            enemySpawner.Disable();
            enemySpawner.ReleasePoolRef<EnemySpawner>(prefabEnemySpawner);
            enemySpawner = null;

            spawningInterval.Clear();
            pushSignal.Clear();
            beat.Clear();

            GunFireSystemCollection.ClearGunSystem();
        }
    }

    [Serializable]
    public class Interval {
        [SerializeField] private float step;
        public event Action trigger;
        private int lastInterval;

        private float GetIntervalLength(float bpm) {
            return 60f / (bpm * step);
        }

        internal void CheckForNewInterval(AudioSource audio, float bpm) {
            float sampledTime = (audio.timeSamples / (audio.clip.frequency * GetIntervalLength(bpm)));

            if (Mathf.FloorToInt(sampledTime) != lastInterval) {
                lastInterval = Mathf.FloorToInt(sampledTime);
                trigger?.Invoke();
            }
        }

        internal void Clear() {
            trigger = null;
            lastInterval = 0;
        }
    }
}