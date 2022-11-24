using UnityEngine;
using TMPro;

namespace GO22
{
    public class ScoreText : MonoBehaviour
    {
        private TMP_Text scoreText;
        void Start()
        {
            scoreText = GetComponent<TMP_Text>();
            scoreText.text = $"Your score\n{ScoreManager.Instance?.Score}";
        }
    }
}
