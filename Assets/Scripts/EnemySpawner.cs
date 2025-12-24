using BulletSystem.GameState;
using Majinfwork.Pool;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BulletSystem {
    public class EnemySpawner : MonoBehaviour {
        [Header("Prefab")]
        public Enemy prefabEnemy;
        public float outerDistance = 8f;
        private List<Enemy> enemies = new();

        private Interval pushSignal;
        private Interval beatSignal;
        public void SetEvent(Interval push, Interval beat) {
            pushSignal = push;
            beatSignal = beat;
        }

        public void Spawn() {
            if (prefabEnemy) {
                Vector3 spawnPoint = Random.onUnitSphere * outerDistance;

                if (prefabEnemy.InstantiatePoolRef(spawnPoint, Quaternion.identity, transform, out Enemy enemy)) {
                    enemy.Initialize(DespawnEnemy);
                    enemies.Add(enemy);
                    pushSignal.trigger += enemy.Push;
                    beatSignal.trigger += enemy.Pulse;
                }
            }
        }

        public void Disable() {
            for(int i = enemies.Count-1; i>=0; i--) {
                enemies[i].DeleteObject();
            }

            pushSignal = null;
            beatSignal = null;
        }

        private void DespawnEnemy(Enemy enemy) {
            pushSignal.trigger -= enemy.Push;
            beatSignal.trigger -= enemy.Pulse;

            enemy.ReleasePoolRef<Enemy>(prefabEnemy);
            enemies.Remove(enemy);
        }

        private void OnDrawGizmos() {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 1.0f);
        }
    }
}