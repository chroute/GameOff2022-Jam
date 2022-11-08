using UnityEngine;

namespace GO22
{
    public class DoctorMovement : DestroyAfterGame
    {
        [SerializeField]
        private float force = 45;
        private Rigidbody2D body;
        private Collider2D col;

        void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            col = GetComponent<Collider2D>();
        }

        void FixedUpdate()
        {
            body.AddForce(new Vector2(force, 0), ForceMode2D.Force);
        }

        void LateUpdate()
        {
            if (!col.IsTouchingLayers(LayerMask.GetMask("Ground")))
            {
                Debug.Log("Win");
                GameManager.Instance?.Win();
            }
        }
    }
}
