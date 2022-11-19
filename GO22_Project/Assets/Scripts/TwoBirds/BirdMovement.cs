using UnityEngine;

namespace GO22
{
    public class BirdMovement : MonoBehaviour
    {
        public float Speed { get; set; }
        private Rigidbody2D rb;
        private Collider2D col;
        private Animator animator;

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            col = GetComponent<Collider2D>();
            animator = GetComponent<Animator>();
        }

        void FixedUpdate()
        {
            rb.velocity = new Vector2(Speed, rb.velocity.y);
        }

        void LateUpdate()
        {
            if (transform.position.x < CamLimitCoordinate.Instance.MinX ||
                transform.position.x > CamLimitCoordinate.Instance.MaxX ||
                transform.position.y < CamLimitCoordinate.Instance.MinY)
            {
                Destroy(this.gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D other) {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.mass = 10;
            col.enabled = false;
            animator.enabled = false;
        }
    }
}
