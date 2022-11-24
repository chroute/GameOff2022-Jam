using UnityEngine.UI;
using UnityEngine;

namespace GO22
{
    public class HealthBar : MonoBehaviour
    {

        private RectTransform barRectTransform;
        private Image barImage;

        void Start()
        {
            barRectTransform = GetComponent<RectTransform>();
            barImage = GetComponent<Image>();
            barImage.color = Color.green;
        }

        private void OnEnable()
        {
            GameManager.playerLoseEvent += UpdateHealth;
        }

        private void OnDisable()
        {
            GameManager.playerLoseEvent -= UpdateHealth;
        }

        public void UpdateHealth(int current, int max)
        {
            float progress = (float)current / (float)max;
            barImage.fillAmount = Mathf.Lerp(0, 1, progress);
            barImage.color = Color.Lerp(Color.red, Color.green, progress);
        }
    }
}
