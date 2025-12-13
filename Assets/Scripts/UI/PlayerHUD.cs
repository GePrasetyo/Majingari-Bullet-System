using ShootingGames.GameState;
using ShootingGames.Gun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ShootingGames.UI {
    public class PlayerHUD : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI stageScoreText;
        [SerializeField] private TextMeshProUGUI comboText;

        [SerializeField] private TextMeshProUGUI ammoText;

        [SerializeField] private Image health;
        [SerializeField] private Image chargingBar;
        [SerializeField] private Image reloadingBar;
        [SerializeField] private Image currentGunIcon;

        [SerializeField] private Sprite[] gunIcons;
        [SerializeField] private Sprite noGunIcon;

        [SerializeField] private GameObject ammoPanel;
        [SerializeField] private Gradient chargeGradient;
        [SerializeField] private float resizeSpeed = 5f;
        [SerializeField] private float maxAdditionalSizePulse = 0.5f;

        //Beat animation
        private Interval beatInterval;

        private float lastHealthPresented;
        private int currentGunIconIndex;
        private int lastScorePrinted;
        private int combo;
        private ushort lastAmmoPrinted;

        public void Initialize(PlayerHUDModel model, InputFireStatus modelFire, Interval beatInterval) {
            stageScoreText.text = $"Score {model.gameScore:00000}";
            comboText.text = "";
            health.fillAmount = model.healthPercentage;
            currentGunIconIndex = model.gunIndex;
            currentGunIcon.sprite = currentGunIconIndex == -1 || currentGunIconIndex >= gunIcons.Length ? noGunIcon : gunIcons[currentGunIconIndex];

            reloadingBar.gameObject.SetActive(modelFire.reloading.HasValue);
            if (modelFire.reloading.HasValue) {
                reloadingBar.fillAmount = 1f - modelFire.reloading.Value;
            }

            chargingBar.gameObject.SetActive(modelFire.charging.HasValue);
            if (modelFire.charging.HasValue) {
                chargingBar.fillAmount = modelFire.charging.Value;
                chargingBar.color = chargeGradient.Evaluate(modelFire.charging.Value);
            }

            ammoPanel.SetActive(modelFire.ammo.HasValue);
            if (modelFire.ammo.HasValue) {
                if (lastAmmoPrinted != modelFire.ammo) {
                    lastAmmoPrinted = (ushort)modelFire.ammo;
                    ammoText.text = modelFire.ammo.ToString();
                }
            }

            this.beatInterval = beatInterval;
            this.beatInterval.trigger += Pulse;
        }

        public void RefreshUI(PlayerHUDModel model) {
            if(lastScorePrinted != model.gameScore) {
                stageScoreText.text = $"Score {model.gameScore:00000}";
            }
            
            if(lastHealthPresented != model.healthPercentage) {
                health.fillAmount = model.healthPercentage;
            }

            if (currentGunIconIndex != model.gunIndex) {
                currentGunIconIndex = model.gunIndex;
                currentGunIcon.sprite = currentGunIconIndex == -1 || currentGunIconIndex >= gunIcons.Length? noGunIcon : gunIcons[currentGunIconIndex];
            }

            if(combo != model.gameCombo) {
                comboText.text = model.gameCombo < 3 ? "" : $"x{model.gameCombo} Combo";
            }

            combo = model.gameCombo;
            stageScoreText.transform.localScale = Vector3.Lerp(stageScoreText.transform.localScale, Vector3.one, Time.deltaTime * resizeSpeed);
        }

        public void RefreshUI(InputFireStatus model) {
            reloadingBar.gameObject.SetActive(model.reloading.HasValue);
            if (model.reloading.HasValue) {
                reloadingBar.fillAmount = 1f - model.reloading.Value;
            }

            chargingBar.gameObject.SetActive(model.charging.HasValue);
            if (model.charging.HasValue) {
                chargingBar.fillAmount = model.charging.Value;
                chargingBar.color = chargeGradient.Evaluate(model.charging.Value);
            }

            ammoPanel.SetActive(model.ammo.HasValue);
            if (model.ammo.HasValue) {
                if(lastAmmoPrinted != model.ammo) {
                    lastAmmoPrinted = (ushort)model.ammo;
                    ammoText.text = model.ammo.ToString();
                }
            }
        }

        private void Pulse() {
            stageScoreText.transform.localScale = Vector3.one * (1 + (maxAdditionalSizePulse * (Mathf.Min(combo / 20f, 1f))));
        }

        public void Disable() {
            this.beatInterval.trigger -= Pulse;
            this.beatInterval = null;
        }
    }

    public struct PlayerHUDModel {
        public float healthPercentage { get; init; }
        public int gameScore { get; init; }
        public int gameCombo { get; init; }
        public int gunIndex { get; init; }
    }
}