using TMPro;
using UnityEngine;

namespace GO22
{
    public class WordDisplay : MonoBehaviour
    {
        private TMP_Text displayText;

        void Start()
        {
            displayText = GetComponent<TMP_Text>();
            displayText.text = WordGenerator.Instance.LoadWord();
        }

        void OnDisable() {
            WordGenerator.Instance.UnLoadWord();
        }
    }
}
