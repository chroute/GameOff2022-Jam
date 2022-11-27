using UnityEngine.InputSystem;
using UnityEngine;

namespace GO22
{
    public class BirdEatsWormMovement : MonoBehaviour
    {
        [SerializeField]
        private float moveSpeed = 5f;

        [SerializeField]
        private float rotationSpeed = 10f;
        [SerializeField]
        private int targetWormsNumber;
        private int wormsCaught;
        private Vector2 input;
        private Rigidbody2D rb;
        private Collider2D col;
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            col = GetComponent<Collider2D>();
        }

        void FixedUpdate()
        {
            rb.velocity = input * moveSpeed;
            if (input != Vector2.zero)
            {
                float targetAngle = Vector2.SignedAngle(Vector2.right, input);
                if (targetAngle >= -90 && targetAngle <= 90)
                {
                    FlipY(1);
                }
                else
                {
                    FlipY(-1);
                }
                Vector3 targetRotation = new Vector3(0, 0, targetAngle);
                rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(targetRotation), rotationSpeed));
            }
        }

        void LateUpdate()
        {
            float clampedX = Mathf.Clamp(transform.position.x, CamLimitCoordinate.Instance.MinX, CamLimitCoordinate.Instance.MaxX);
            float clampedY = Mathf.Clamp(transform.position.y, CamLimitCoordinate.Instance.MinY, CamLimitCoordinate.Instance.MaxY);
            transform.position = new Vector2(clampedX, clampedY);
        }

        void OnMove(InputValue inputValue)
        {
            input = inputValue.Get<Vector2>();
        }

        private void OnTriggerEnter2D(Collider2D other) {
            Destroy(other.gameObject);
            if (++wormsCaught >= targetWormsNumber) {
                GameManager.Instance?.Win();
            }
        }

        void FlipY(int y) {
            transform.localScale = new Vector3(transform.localScale.x, y, transform.localScale.z);
        }
    }
}
