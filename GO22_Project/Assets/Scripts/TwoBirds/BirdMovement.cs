using UnityEngine;

namespace GO22
{
    public class BirdMovement : MonoBehaviour
    {
        public float Speed { get; set; }
        private Rigidbody2D rb;
        private Collider2D col;
        private Animator animator;
        private AudioManager audioPlayer;
        private SpriteRenderer spriteRenderer;


        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            col = GetComponent<Collider2D>();
            animator = GetComponent<Animator>();
            audioPlayer = FindObjectOfType<AudioManager>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        void FixedUpdate()
        {
            rb.velocity = new Vector2(Speed, rb.velocity.y);
        }

        void LateUpdate()
        {
            if (CamLimitCoordinate.Instance.IsOutOfLimit(transform.position))
            {
                Destroy(this.gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D other) {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.mass = 10;
            col.enabled = false;
            animator.enabled = false;
            spriteRenderer.flipY = true;
            audioPlayer.Play("Hit");
            audioPlayer.Play("Falling");

        }
    }
}
