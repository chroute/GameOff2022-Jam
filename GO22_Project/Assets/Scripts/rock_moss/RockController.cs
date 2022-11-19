using UnityEngine;
namespace GO22

{
    public class RockController : MonoBehaviour
    {
        public Vector2 jump;
        public float jumpForce = 2.0f;
        public bool isGrounded;
        Rigidbody2D rb;

        void Start()
        {
            CamFollow camFollow = Camera.main.GetComponent<CamFollow>();
            camFollow?.FollowMe(transform);
            rb = GetComponent<Rigidbody2D>();
            jump = new Vector2(0.0f, 2.0f);
        }

        private void OnCollisionStay2D(Collision2D other)
        {
            isGrounded = true;
        }

        private void OnDisable() {
            CamFollow camFollow = Camera.main.GetComponent<CamFollow>();
            camFollow?.unFollow();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {

                rb.AddForce(jump * jumpForce, ForceMode2D.Impulse);
                isGrounded = false;

                isStopped();
            }
        }

        private void isStopped()
        {
            if (rb.velocity.y < 1f)
            {
                GameManager.Instance?.Lose();
            }
        }
    }
}