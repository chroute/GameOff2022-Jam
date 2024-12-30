using System;
using System.Collections;
using UnityEngine;

namespace GO22
{
    public class CardTurn : MonoBehaviour
    {
        [SerializeField]
        private float cardTurnDuration = 0.5f;
        [SerializeField]
        private float tiltDegree = 15f;
        private SpriteRenderer cardFaceSprite;
        private SpriteRenderer cardBackSprite;
        public bool FlashCardEnded { get; private set; }

        private void Start()
        {
            cardFaceSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
            cardFaceSprite.flipX = true;
            cardBackSprite = GetComponent<SpriteRenderer>();
            ShowBack();
        }

        public void OnStartEvent(object sender, EventArgs eventArgs)
        {
            StartCoroutine(FlashCard());
        }

        private void OnEnable()
        {
            GameManager.startGameEvent += OnStartEvent;
        }

        private void OnDisable()
        {
            GameManager.startGameEvent -= OnStartEvent;
        }

        public void ShowCard(Action followUpAction)
        {
            StartCoroutine(TurnCard(0f, 180f, followUpAction));
        }

        public void TiltCard()
        {
            Vector3 eulerAngles = transform.rotation.eulerAngles;
            cardFaceSprite.flipX = false;
            transform.rotation = Quaternion.Euler(eulerAngles.x, eulerAngles.x, eulerAngles.z + tiltDegree);
        }

        public IEnumerator FlashCard()
        {
            yield return TurnCard(0f, 180f, null);
            yield return TurnCard(180f, 0f, () => FlashCardEnded = true);
        }

        IEnumerator TurnCard(float from, float to, Action followUpAction)
        {
            float duration = 0;
            bool faceUp = cardFaceSprite.enabled;
            Vector3 initialAngles = transform.rotation.eulerAngles;
            while (duration < cardTurnDuration)
            {
                float angle = Mathf.Lerp(from, to, duration / cardTurnDuration);
                transform.rotation = Quaternion.Euler(initialAngles.x, angle, initialAngles.z);
                if (angle > 90)
                {
                    ShowFace();
                }
                else
                {
                    ShowBack();
                }
                duration += Time.deltaTime;

                yield return null;
            }
            transform.rotation = Quaternion.Euler(initialAngles.x, to, initialAngles.z);
            followUpAction?.Invoke();
        }

        void ShowFace()
        {
            cardFaceSprite.enabled = true;
            cardBackSprite.enabled = false;
        }

        void ShowBack()
        {
            cardFaceSprite.enabled = false;
            cardBackSprite.enabled = true;
        }
    }
}
