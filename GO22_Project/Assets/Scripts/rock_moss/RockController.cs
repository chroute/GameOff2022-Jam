using System;
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
            rb.isKinematic = true;
            jump = new Vector2(0.0f, 2.0f);
        }

        void OnEnable() {
            GameManager.startGameEvent += OnStart;    
        }

        void OnStart(object sender, EventArgs eventArgs) {
            rb.isKinematic = false;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            AudioManager.Instance?.Play("RockHit");
            isGrounded = true;
        }

        private void OnDisable() {
            CamFollow camFollow = Camera.main?.GetComponent<CamFollow>();
            camFollow?.unFollow();
            GameManager.startGameEvent -= OnStart;    
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