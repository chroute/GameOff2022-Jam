using UnityEngine;

namespace GO22
{
    public class WormMovement : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField]
        private float maxSqrDistFromOriginalPos;
        [SerializeField]
        private float minSqrDistFromOriginalPos;
        [SerializeField]
        private float moveSpeed;
        private Rigidbody2D rb;
        private Vector3 originalPosition;
        private Vector2 moveDirection;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            originalPosition = transform.position;
            MoveRandomDirection();
        }

        void FixedUpdate() {
            rb.velocity = moveDirection * moveSpeed;
        }

        void LateUpdate() {
            float sqrDistance = (transform.position - originalPosition).sqrMagnitude;
            if (sqrDistance > maxSqrDistFromOriginalPos) {
                MoveTowordsOriginalPosition();
            } else if (sqrDistance < minSqrDistFromOriginalPos) {
                MoveRandomDirection();
            }
        }

        void MoveRandomDirection() {
            moveDirection = Random.insideUnitCircle.normalized;
        }

        void MoveTowordsOriginalPosition() {
            moveDirection = originalPosition - transform.position;
        }
    }
}
