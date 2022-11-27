using System.Collections;
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
        [SerializeField]
        private float pitchIncrease = 1.25f;
        [SerializeField]
        private float blendSoundDuration = 0.1f;

        private Rigidbody2D body;
        private JuiceFiller bottleFiller;
        private Animator animator;
        private Vector2 input;
        private bool blenderBroken;
        private AudioSource audioSource;
        private AudioManager audioPlayer; // for minor, instantiated sound effects


        void Start()
        {
            lemonCaught = 0;
            blenderBroken = false;
            body = GetComponent<Rigidbody2D>();
            bottleFiller = GetComponentInChildren<JuiceFiller>();
            animator = GetComponentInChildren<Animator>();
            audioSource = GetComponent<AudioSource>();
            audioPlayer = FindObjectOfType<AudioManager>();
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
                StartCoroutine(ChangePitch());
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
                audioSource.Stop();
                audioPlayer.Play("RockHit");
                GameManager.Instance?.Lose();
            }
        }

        private IEnumerator ChangePitch()
        {
            audioSource.pitch = pitchIncrease;
            yield return new WaitForSeconds(blendSoundDuration);
            audioSource.pitch = 1f;

        }

    }
}
