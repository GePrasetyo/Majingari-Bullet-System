namespace ShootingGames.Gun {
    public abstract class GunFireSystem : ITickObject {
        public virtual void Initialize() {
            GameInstance.Instance.RegisterTick(this);
        }
        public virtual void Disable() {
            GameInstance.Instance.UnRegisterTick(this);
        }

        public abstract void Fire(ProjectileData rawData);
        public abstract void Tick();
    }
}