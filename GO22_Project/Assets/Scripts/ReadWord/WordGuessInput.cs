using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

namespace GO22
{
    public class WordGuessInput : MonoBehaviour
    {
        private string guess;
        private TMP_InputField inputField;
        private GameObject prevEventSystemSelectedGO;
        [SerializeField]
        private WordDisplay wordDisplay;

        void Start()
        {
            inputField = GetComponent<TMP_InputField>();
            inputField.onEndEdit.AddListener(fieldValue => guess = fieldValue);
            Canvas canvas = GetComponentInParent<Canvas>();
            canvas.worldCamera = Camera.main;
            EventSystem.current.SetSelectedGameObject(this.gameObject, null);
        }

        void compareGuessToTarget()
        {
            if (wordDisplay.currentWord == null)
            {
                return;
            }

            if (wordDisplay.currentWord.Equals(guess))
            {
                GameManager.Instance?.Win();
            }
            else
            {
                GameManager.Instance?.Lose();
            }
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                EventSystem.current.SetSelectedGameObject(this.gameObject, null);
            }

            if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
            {
                compareGuessToTarget();
            }
        }
    }
}
