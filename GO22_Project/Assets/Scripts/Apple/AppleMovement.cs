using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GO22
{
    public class AppleMovement : MonoBehaviour
    {
        [SerializeField]
        private float force = 15;
        private Rigidbody2D body;
        private Vector2 input;
        private PlayerInput playerInput;


        void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            playerInput = GetComponent<PlayerInput>();
        }

        void OnEnable() {
            GameManager.playerWinEvent += OnWin;    
        }

        void OnDisable() {
            GameManager.playerWinEvent -= OnWin;
        }

        void OnMove(InputValue inputValue)
        {
            input = inputValue.Get<Vector2>();
            body.AddForce(new Vector2(input.x * force, 0), ForceMode2D.Impulse);
        }

        void LateUpdate()
        {
            if (transform.position.x > PlatformWidth.Instance.Width / 2)
            {
                GameManager.Instance?.Lose();
                body.velocity = new Vector2(0, 0);
                DisableInput();
            }
        }

        void OnWin(object sender, EventArgs eventArgs) {
            DisableInput();
            // TODO change animation
        }

        void DisableInput() {
            playerInput.currentActionMap.Disable();
        }
    }
}
