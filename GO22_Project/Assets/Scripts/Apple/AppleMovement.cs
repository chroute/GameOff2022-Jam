using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GO22
{
    public class AppleMovement : MonoBehaviour
    {
        [SerializeField]
        private float pushForce = 5;
        [SerializeField]
        private float walkSpeed = 5;

        private const string IS_MOVING = "isMoving";
        private const string IS_PUSHING = "isPushing";
        private const string DOCTOR = "Doctor";
        private const string BOUNDARIES = "boundaries";

        private Rigidbody2D body;
        private Vector2 input;
        private PlayerInput playerInput;
        private Animator animator;
        private bool isPushing;

        void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            playerInput = GetComponent<PlayerInput>();
            animator = GetComponent<Animator>();
            DisableMove();
        }

        void OnEnable()
        {
            GameManager.playerWinEvent += OnWin;
            GameManager.playerLoseEvent += OnLose;
            GameManager.startGameEvent += OnStart;
        }

        void OnDisable()
        {
            GameManager.playerWinEvent -= OnWin;
            GameManager.playerLoseEvent -= OnLose;
            GameManager.startGameEvent -= OnStart;
        }

        void OnMove(InputValue inputValue)
        {
            input = inputValue.Get<Vector2>();
            if (isPushing)
            {
                body.AddForce(new Vector2(input.x * pushForce, 0), ForceMode2D.Impulse);
            }
        }

        void FixedUpdate()
        {
            if (!isPushing)
            {

                body.velocity = new Vector2(input.x * walkSpeed, body.velocity.y);
                bool hasVelocity = Mathf.Abs(body.velocity.x) > Mathf.Epsilon;
                if (hasVelocity)
                {
                    transform.localScale = new Vector3(Math.Sign(body.velocity.x), 1);
                    animator.SetBool(IS_MOVING, true);
                }
                else
                {
                    animator.SetBool(IS_MOVING, false);
                }
            }

        }

        void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag(DOCTOR))
            {
                transform.localScale = new Vector3(Mathf.Sign(
                    other.gameObject.transform.position.x - transform.position.x), 1);
                animator.SetBool(IS_PUSHING, true);
                isPushing = true;
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.gameObject.CompareTag(DOCTOR))
            {
                animator.SetBool(IS_PUSHING, false);
                isPushing = false;
            }
        }

        private void OnTriggerEnter2D(Collider2D other) 
        {
            if (other.gameObject.CompareTag(BOUNDARIES))
            {
                GameManager.Instance?.Lose();
            }

        }

        void OnLose(int currentLife, int initialLife)
        {
            DisableMove();

        }

        void OnWin(object sender, EventArgs eventArgs)
        {
            DisableMove();
        }

        void OnStart(object sender, EventArgs eventArgs) {
            EnableMove();
        }

        void EnableMove()
        {
            playerInput.currentActionMap.Enable();
        }

        void DisableMove()
        {
            isPushing = false;
            playerInput.currentActionMap.Disable();
            body.velocity = new Vector2(0, 0);
        }
    }
}
