using UnityEngine;

namespace ShootingGames {
    public class GameStats {
        public int gameScore { get; private set; } = 0;
        public int combo { get; private set; } = 0;
        public int maxCombo { get; private set; } = 0;

        public void ResetScore() {
            gameScore = 0;
            combo = 0;
            maxCombo = 0;
        }

        public void ResetCombo() {
            combo = 0;
        }

        public void AddScore(int a_value) {
            gameScore += a_value;
            combo++;

            maxCombo = Mathf.Max(maxCombo, combo);
        }

        public GameStats Clone() {
            return this.MemberwiseClone() as GameStats;
        }
    }
}