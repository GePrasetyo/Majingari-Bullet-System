using TMPro;
using UnityEngine;

namespace BulletSystem.UI {
    public class ResultHUD : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI resultScore;
        [SerializeField] private TextMeshProUGUI maxCombo;

        public void Setup(GameStats stats) {
            if(stats != null) {
                resultScore.text = $"Last Score {stats.gameScore:00000}";
                maxCombo.text = $"Highest Combo : x{stats.maxCombo}";
            }
            else {
                resultScore.text = string.Empty;
            }
        }
    }
}