using UnityEngine;

namespace GO22
{
    public class MonkeyMovement : MonoBehaviour
    {
        [SerializeField]
        private Sprite idle;
        [SerializeField]
        private Sprite left;
        [SerializeField]
        private Sprite up;
        [SerializeField]
        private Sprite right;
        [SerializeField]
        private Sprite down;
        private SpriteRenderer image;

        void Awake() {
            image = GetComponent<SpriteRenderer>();

        }

        public void Idle() {
            image.sprite = idle;
        }

        public void Move(int moveIndex) {
            switch (moveIndex) {
                case 0:
                    image.sprite = left;
                    AudioManager.Instance?.Play("VoiceLeft");
                    break;
                case 1:
                    image.sprite = up;
                    AudioManager.Instance?.Play("VoiceUp");
                    break;
                case 2:
                    image.sprite = right;
                    AudioManager.Instance?.Play("VoiceRight");
                    break;
                case 3:
                    image.sprite = down;
                    AudioManager.Instance?.Play("VoiceDown");
                    break;
            }
        }
    }
}
