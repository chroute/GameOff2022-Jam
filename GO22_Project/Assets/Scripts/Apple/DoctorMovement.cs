using UnityEngine;
using UnityEngine.InputSystem;

namespace GO22
{
    public class DoctorMovement : MonoBehaviour
    {
        [SerializeField]
        private float force = 45;
        private Rigidbody2D body;
        private bool shouldPush;

        void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            shouldPush = true;
        }

        void FixedUpdate()
        {
            if (shouldPush) {
                body.AddForce(new Vector2(force, 0), ForceMode2D.Force);
            }
        }

        void LateUpdate()
        {
            if (transform.position.x < -PlatformWidth.Instance.Width / 2)
            {
                GameManager.Instance?.Win();
                body.velocity = new Vector2(0, 0);
                shouldPush = false;
            }
        }
    }
}
