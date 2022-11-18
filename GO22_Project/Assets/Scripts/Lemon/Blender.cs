using UnityEngine;
using UnityEngine.InputSystem;

namespace GO22
{
    public class Blender : MonoBehaviour
    {
        [SerializeField]
        private float moveSpeed = 2;
        [SerializeField]
        private int targetLemon = 8;
        private int lemonCaught = 0;

        private Rigidbody2D body;
        private JuiceFiller bottleFiller;
        private Vector2 input;
        
        void Start()
        {
            lemonCaught = 0;
            body = GetComponent<Rigidbody2D>();
            bottleFiller = GetComponentInChildren<JuiceFiller>();
        }
        
        void FixedUpdate() {
            body.velocity = new Vector2(input.x * moveSpeed, body.velocity.y);
        
        }

        void OnMove(InputValue inputValue)
        {
            input = inputValue.Get<Vector2>();
        }

        void OnCollisionEnter2D(Collision2D other) {
            Destroy(other.gameObject);
            lemonCaught++;
            bottleFiller?.FillBottle(targetLemon, lemonCaught);
        }
    }
}
