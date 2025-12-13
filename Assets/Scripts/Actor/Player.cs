using BulletSystem.GameState;
using BulletSystem.Gun;
using BulletSystem.Movement;
using Majingari.Framework;
using Majingari.Framework.Pool;
using BulletSystem.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BulletSystem {
    public class Player : Actor, IHitable {
        public const byte Team = 1;
        private PlayerController playerController;

        [ClassReference, SerializeReference] private BaseActorMovement movement;
        [SerializeField] private PostMovement[] postMovement = Array.Empty<PostMovement>();

        [Header("Gun")]
        public GunFireFactory[] bulletFactories;
        public Transform gunPoint;

        private List<BaseGun> guns = new List<BaseGun>();
        private List<InputFireSystem> inputGunSystems = new List<InputFireSystem>();

        private BaseGun currentGun;
        private InputFireSystem currentInputGun;

        private HitID hitID;

        [Header("UI")]
        public PlayerHUD prefabHud;
        private PlayerHUD hud;

        [field: SerializeField] public float maxHealth { get; private set; } = 5;
        [field: SerializeField] public float health { get; private set; } = 5;

        public int gunIndex { get; private set; } = 0;
        private int totalGun;

        public void Initialize(PlayerController controller, Interval beatInterval) {
            playerController = controller;
            ResetStats();

            totalGun = bulletFactories.Length;
            for (int i = 0; i < totalGun; i++) {
                guns.Add(bulletFactories[i].CreateGun(gunPoint));
                inputGunSystems.Add(bulletFactories[i].CreateInputSystem());
            }

            SwitchGun();

            if (prefabHud.InstantiatePoolRef(out hud)) {
                hud.gameObject.SetActive(true);
                
                currentInputGun.InputTick(currentGun, out InputFireStatus fireStatus);
                hud.Initialize(new PlayerHUDModel() {
                    healthPercentage = health / maxHealth,
                    gameScore = GameInstance.Instance?.Stats?.gameScore ?? 0,
                    gameCombo = GameInstance.Instance?.Stats?.combo ?? 0,
                    gunIndex = gunIndex,
                }, fireStatus, beatInterval);
            }

            hitID = new HitID() {
                ID = Guid.NewGuid(),
                Team = Team
            };
        }

        private void Update() {
            if (playerController != null) {
                movement?.Move(playerController.Input.MainAxis, out Vector2 moveDirection);
            }

            if(currentInputGun != null) {
                currentInputGun.InputTick(currentGun, out InputFireStatus fireStatus);
                hud?.RefreshUI(fireStatus);

                if (fireStatus.fire) {
                    currentGun.Fire(hitID);
                }
            }

            if (playerController.Input.isSwitchGunPressed) {
                gunIndex = gunIndex == totalGun-1 ? 0 : gunIndex + 1;
                SwitchGun();
            }
        }

        private void LateUpdate() {
            for(int i=0; i<postMovement.Length; i++) {
                postMovement[i].mover.PostMovement();
            }

            hud.RefreshUI(new PlayerHUDModel() {
                healthPercentage = health / maxHealth,
                gameScore = GameInstance.Instance?.Stats?.gameScore ?? 0,
                gameCombo = GameInstance.Instance?.Stats?.combo ?? 0,
                gunIndex = gunIndex,
            });
        }

        private void OnDisable() {
            for (int i = 0; i < inputGunSystems.Count; i++) {
                inputGunSystems[i]?.Dispose();
            }

            for (int i = 0; i < guns.Count; i++) {
                guns[i]?.Dispose();
            }

            guns.Clear();
            inputGunSystems.Clear();

            hud.Disable();
            hud.Release<PlayerHUD>(prefabHud);
            hud = null;
        }

        private void SwitchGun() {
            currentGun = guns[gunIndex];
            currentInputGun = inputGunSystems[gunIndex];
        }

        public bool IsHitValid(HitID id) {
            return id.Team != Team;
        }

        public void OnHit() {
            ApplyDamage(1f);
        }

        public void Heal(float value) {
            health = Mathf.Min(health + value, maxHealth);
        }

        public void ApplyDamage(float value) {
            health = Mathf.Max(health - value, 0);
            GameInstance.Instance?.Stats.ResetCombo();
        }

        public void ResetStats() {
            health = maxHealth;
            gunIndex = 0;
        }
    }
}