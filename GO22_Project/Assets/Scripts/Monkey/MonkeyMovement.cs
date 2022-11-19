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
                    break;
                case 1:
                    image.sprite = up;
                    break;
                case 2:
                    image.sprite = right;
                    break;
                case 3:
                    image.sprite = down;
                    break;
            }
        }
    }
}
