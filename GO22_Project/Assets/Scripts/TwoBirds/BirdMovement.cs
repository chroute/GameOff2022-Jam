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

        void LateUpdate()
        {
            if (transform.position.x < CamLimitCoordinate.Instance.MinX ||
                transform.position.x > CamLimitCoordinate.Instance.MaxX)
            {
                Destroy(this.gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D other) {
            Destroy(this.gameObject);
        }
    }
}
