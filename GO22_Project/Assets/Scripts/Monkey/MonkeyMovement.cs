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
        private AudioManager audioPlayer;

        void Awake() {
            image = GetComponent<SpriteRenderer>();
            audioPlayer = FindObjectOfType<AudioManager>();

        }

        public void Idle() {
            image.sprite = idle;
        }

        public void Move(int moveIndex) {
            switch (moveIndex) {
                case 0:
                    image.sprite = left;
                    audioPlayer.Play("VoiceLeft");
                    break;
                case 1:
                    image.sprite = up;
                    audioPlayer.Play("VoiceUp");
                    break;
                case 2:
                    image.sprite = right;
                    audioPlayer.Play("VoiceRight");
                    break;
                case 3:
                    image.sprite = down;
                    audioPlayer.Play("VoiceDown");
                    break;
            }
        }
    }
}
