using UnityEngine;
using UnityEngine.InputSystem;

namespace GO22
{
    public class AppleMovement : DestroyAfterGame
    {
        [SerializeField]
        private float force = 15;
        private Rigidbody2D body;
        private Vector2 input;

        void Awake()
        {
            body = GetComponent<Rigidbody2D>();
        }

        void OnMove(InputValue inputValue)
        {
            input = inputValue.Get<Vector2>();
            body.AddForce(new Vector2(input.x * force, 0), ForceMode2D.Impulse);
        }
    }
}
