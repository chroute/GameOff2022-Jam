using UnityEngine;

namespace GO22
{
    public class BirdMovement : MonoBehaviour
    {
        public float Speed { get; set; }
        private Rigidbody2D rb;

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        void FixedUpdate()
        {
            rb.velocity = new Vector2(Speed, rb.velocity.y);
        }
    }
}
