using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GO22
{
    public class StoneMovement : MonoBehaviour
    {
        private const string BLUE_BIRD = "BlueBird";
        private const string RED_BIRD = "RedBird";

        [SerializeField]
        private float moveSpeed = 5f;
        [SerializeField]
        private float throwSpeed = 10f;
        [SerializeField]
        private Vector3 originalPos = new Vector3(0, -5, 0);

        private Rigidbody2D body;
        private PlayerInput playerInput;

        private Vector2 input;

        private bool thrown;
        private BirdHit birdHit;

        void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            playerInput = GetComponent<PlayerInput>();
        }

        void Start() {
            reset();    
        }

        void OnEnable() {
            GameManager.playerLoseEvent += OnLose;    
        }

        void OnDisable() {
            GameManager.playerLoseEvent -= OnLose;    
        }

        void OnMove(InputValue inputValue)
        {
            input = inputValue.Get<Vector2>();
        }

        void OnThrow(InputValue inputValue)
        {
            if (inputValue.isPressed)
            {
                thrown = true;
            }
        }

        void FixedUpdate()
        {
            if (!thrown)
            {
                MoveStone();
            }
            else
            {
                ThrowStone();
            }
        }

        void LateUpdate()
        {
            if (transform.position.y > CamLimitCoordinate.Instance.MaxY)
            {
                reset();
            }
        }

        void MoveStone()
        {
            Vector2 vel = new Vector2(input.x * moveSpeed, body.velocity.y);
            body.velocity = vel;
        }

        void ThrowStone()
        {
            Vector2 vel = new Vector2(0, throwSpeed);
            body.velocity = vel;
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(BLUE_BIRD)) {
                birdHit |= BirdHit.BlueBird;
            } else if (other.CompareTag(RED_BIRD)) {
                birdHit |= BirdHit.RedBird;
            }

            if (birdHit == BirdHit.All)
            {
                GameManager.Instance?.Win();
                DisableInput();
            }
        }

        void reset()
        {
            thrown = false;
            body.velocity = new Vector2(0, 0);
            birdHit = BirdHit.None;
            transform.position = originalPos;
        }

        void OnLose(object sender, EventArgs eventArgs) {
            DisableInput();
        }

        void DisableInput() {
            playerInput.currentActionMap.Disable();
        }
    }

    public enum BirdHit {
        None = 0,
        BlueBird = 1,
        RedBird = 2,
        All = 3
    }
}
