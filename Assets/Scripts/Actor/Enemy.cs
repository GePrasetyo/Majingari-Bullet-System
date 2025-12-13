using ShootingGames.Movement;
using ShootingGames.Utils;
using System;
using UnityEngine;

namespace ShootingGames {
    public class Enemy : Actor, IHitable {
        public const byte Team = 2;
        [SerializeField] private Material aliveMaterial;
        [SerializeField] private Material activeMaterial;
        [SerializeField] private Material deadMaterial;
        [SerializeField] private Renderer render;

        [ClassReference, SerializeReference] private BaseActorMovement movement;
        [SerializeField] private PostMovement[] postMovement = Array.Empty<PostMovement>();

        [Header("Parameter")]
        public Vector3 originalSize;
        public Vector3 pulseSize;
        public Vector3 deadSize;
        private Vector3 initialSize;

        public float resizeSpeed = 5;
        public float lifeTime = 5;
        public int m_score = 100;

        private event Action<Enemy> onLifeEnd;
        private Vector3 direction;
        private HitID collisionHitID;
        [SerializeField] private BeatStatus beatStatus;

        private float lifeTimeTimer;

        private enum BeatStatus { Alive, Active, Dead }

        private void Start() {
            collisionHitID = new() {
                ID = Guid.NewGuid(),
                Team = Team
            };
        }

        private void Update() {
            //lifetime
            lifeTimeTimer -= Time.deltaTime;
            if (lifeTimeTimer <= 0) {
                DeleteObject();
            }
        }

        private void LateUpdate() {
            if (beatStatus == BeatStatus.Dead || lifeTimeTimer <= 0) {
                return;
            }

            for (int i = 0; i < postMovement.Length; i++) {
                postMovement[i].mover.PostMovement();
            }

            //Scale
            transform.localScale = Vector3.Lerp(transform.localScale, initialSize, Time.deltaTime * resizeSpeed);
            initialSize = Vector3.Lerp(originalSize, Vector3.one * 0.2f, Mathf.Clamp(lifeTimeTimer / lifeTime, 0, 1));
        }

        public void Initialize(Action<Enemy> onLifeEnd) {
            this.onLifeEnd = onLifeEnd;
            lifeTimeTimer = lifeTime;
            direction = (Vector3.zero - transform.localPosition).normalized;
            initialSize = originalSize;
            transform.localScale = initialSize;
            SetStatus(BeatStatus.Alive);
        }

        public void DeleteObject() {
            onLifeEnd?.Invoke(this);
            onLifeEnd = null;
        }

        public void Push() {
            movement.Move(new Vector2(direction.x, direction.y), out Vector2 move);
        }

        public void Pulse() {
            transform.localScale = pulseSize;
        }

        public bool IsHitValid(HitID id) {
            return id.Team != Team && beatStatus == BeatStatus.Alive;
        }

        public void OnHit() {
            GameInstance.Instance?.Stats.AddScore(m_score);
            SetStatus(BeatStatus.Active);
        }

        private void SetStatus(BeatStatus status) {
            beatStatus = status;

            switch (status) {
                case BeatStatus.Active:
                    render.material = activeMaterial;
                    break;
                case BeatStatus.Alive:
                    render.material = aliveMaterial;
                    break;
                case BeatStatus.Dead:
                    render.material = deadMaterial;
                    transform.localScale = deadSize;
                    break;
            }
        }

        private void OnCollisionEnter(Collision collision) {
            if (beatStatus != BeatStatus.Alive) {
                return;
            }

            if (collision.gameObject.TryGetComponent(out IHitable hitTarget)) {
                if (hitTarget.IsHitValid(collisionHitID)) {
                    hitTarget.OnHit();
                    SetStatus(BeatStatus.Dead);
                }
            }

            direction = Vector3.Reflect(direction, collision.GetContact(0).normal).normalized;
        }
    }
}