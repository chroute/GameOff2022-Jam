using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace GO22
{
    public class ProgressBar : MonoBehaviour
    {
        private Image barImage;
        
        void Start()
        {
            barImage = GetComponent<Image>();
        }

        public void ResetProgress() {
            barImage.fillAmount = 0;
        }

        public void UpdateProgress(float current, float max)
        {
            float progress = current / max;
            barImage.fillAmount = Mathf.Lerp(0, 1, progress);
        }
    }
}
