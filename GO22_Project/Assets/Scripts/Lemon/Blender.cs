using UnityEngine;
using UnityEngine.InputSystem;

namespace GO22
{
    public class Blender : MonoBehaviour
    {
        private const string LEMON = "Lemon";
        [SerializeField]
        private float moveSpeed = 8;
        [SerializeField]
        private int targetLemon = 5;
        private int lemonCaught = 0;

        private Rigidbody2D body;
        private JuiceFiller bottleFiller;
        private Animator animator;
        private Vector2 input;
        private bool blenderBroken;

        void Start()
        {
            lemonCaught = 0;
            blenderBroken = false;
            body = GetComponent<Rigidbody2D>();
            bottleFiller = GetComponentInChildren<JuiceFiller>();
            animator = GetComponentInChildren<Animator>();
        }

        void FixedUpdate()
        {
            body.velocity = new Vector2(input.x * moveSpeed, body.velocity.y);

        }

        void LateUpdate()
        {
            Vector3 clampedPos = transform.position;
            clampedPos.x = Mathf.Clamp(clampedPos.x, CamLimitCoordinate.Instance.MinX, CamLimitCoordinate.Instance.MaxX);
            transform.position = clampedPos;
        }

        void OnMove(InputValue inputValue)
        {
            input = inputValue.Get<Vector2>();
        }

        void OnCollisionEnter2D(Collision2D other)
        {
            if (blenderBroken)
            {
                return;
            }

            if (other.gameObject.CompareTag(LEMON))
            {
                Destroy(other.gameObject);
                lemonCaught++;
                bottleFiller?.FillBottle(targetLemon, lemonCaught);
                if (lemonCaught == targetLemon)
                {
                    GameManager.Instance?.Win();
                }
            }
            else
            {
                // TODO: make blender trip over
                blenderBroken = true;
                animator.enabled = false;
                GameManager.Instance?.Lose();
            }
        }
    }
}
