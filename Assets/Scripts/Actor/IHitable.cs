using System;

namespace ShootingGames {
    public interface IHitable {
        public bool IsHitValid(HitID id);
        public void OnHit();
    }

    public struct HitCollisionData {
        public HitID HitID;
    }

    public struct HitID {
        public Guid ID { get; init; }
        public int Team { get; init; }
    }
}