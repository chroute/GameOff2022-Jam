using TMPro;
using UnityEngine;

namespace GO22
{
    public class WordDisplay : MonoBehaviour
    {
        private TMP_Text displayText;
        public string currentWord;

        void Start()
        {
            displayText = GetComponent<TMP_Text>();
            currentWord = WordGenerator.Instance.PickWord();
            displayText.text = currentWord;
        }
    }
}
