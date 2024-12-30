using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

namespace GO22
{
    public class WordGuessInput : MonoBehaviour
    {
        private string currentInput = "";
        private TMP_InputField inputField;
        private GameObject prevEventSystemSelectedGO;
        [SerializeField]
        private WordDisplay wordDisplay;
        
        private bool hasWon = false; // Prevent multiple win triggers

        void Start()
        {
            inputField = GetComponent<TMP_InputField>();
            
            // Use onValueChanged instead of onEndEdit to check continuously
            inputField.onValueChanged.AddListener(OnInputValueChanged);
            
            Canvas canvas = GetComponentInParent<Canvas>();
            canvas.worldCamera = Camera.main;
            EventSystem.current.SetSelectedGameObject(this.gameObject, null);
            ForceFocus();
        }

        void OnInputValueChanged(string value)
        {
            currentInput = value;
            CheckGuess();
        }

        void CheckGuess()
        {
            if (wordDisplay.currentWord == null || hasWon)
            {
                return;
            }

            // Compare the current input with the target word
            if (wordDisplay.currentWord.Equals(currentInput, System.StringComparison.OrdinalIgnoreCase)) // Added case-insensitive comparison
            {
                hasWon = true;
                GameManager.Instance?.Win();
            }
        }

        void ForceFocus()
        {
            // Force selection of the input field
            EventSystem.current.SetSelectedGameObject(this.gameObject, null);
            inputField.ActivateInputField();
        }

        void Update()
        {
            // Force focus every frame if the input field isn't selected
            if (!inputField.isFocused)
            {
                ForceFocus();
            }
            // Keep input field selected
            if (Input.GetMouseButtonDown(0))
            {
                EventSystem.current.SetSelectedGameObject(this.gameObject, null);
            }
        }
    }
}
