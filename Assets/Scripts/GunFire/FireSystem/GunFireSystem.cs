using Majinfwork;

namespace BulletSystem.Gun {
    public abstract class GunFireSystem : ITickObject {
        public virtual void Initialize() {
            //ServiceLocator.Resolve<TickSignal>().RegisterTick(this);
        }
        public virtual void Disable() {
            //ServiceLocator.Resolve<TickSignal>().UnRegisterTick(this);
        }

        public abstract void Fire(ProjectileData rawData);
        public abstract void Tick();
    }
}