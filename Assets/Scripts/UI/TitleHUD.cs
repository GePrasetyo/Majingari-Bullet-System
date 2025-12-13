using TMPro;
using UnityEngine;

namespace ShootingGames.UI {
    public class TitleHUD : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI lastScoreText;

        public void Setup(GameStats stats) {
            if(stats != null) {
                lastScoreText.text = $"Last Score {stats.gameScore:00000}";
            }
            else {
                lastScoreText.text = string.Empty;
            }
        }
    }
}