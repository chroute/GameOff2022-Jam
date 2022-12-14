using System;
using UnityEngine;

namespace GO22
{
    public class DoctorMovement : MonoBehaviour
    {
        [SerializeField]
        private float force = 50;
        private const string APPLE = "Apple";
        private const string IS_PUSHING = "isPushing";
        private const string BOUNDARIES = "boundaries";

        private Rigidbody2D body;
        private Animator animator;
        private bool shouldMove;


        void Awake()
        {
            body = GetComponent<Rigidbody2D>();
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

        void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag(APPLE))
            {
                animator.SetBool(IS_PUSHING, true);
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.gameObject.CompareTag(APPLE))
            {
                animator.SetBool(IS_PUSHING, false);
            }
        }

        private void OnTriggerEnter2D(Collider2D other) 
        {
            if (other.gameObject.CompareTag(BOUNDARIES))
            {
                GameManager.Instance?.Win();
            }

        }

        void FixedUpdate()
        {
            if (shouldMove)
            {
                body.AddForce(new Vector2(force, 0), ForceMode2D.Force);
            }
        }

        void OnLose(int currentLife, int initialLife)
        {
            DisableMove();
        }

        void OnWin(object sender, EventArgs eventArgs)
        {
            DisableMove();
            AudioManager.Instance?.Play("WhihelmScream");
        }

        void OnStart(object sender, EventArgs eventArgs)
        {
            EnableMove();
        }

        void DisableMove()
        {
            body.velocity = new Vector2(0, 0);
            shouldMove = false;
        }

        void EnableMove()
        {
            shouldMove = true;
        }
    }
}
