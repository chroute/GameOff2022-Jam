using UnityEngine;

namespace GO22
{
    public class BirdSpawner : MonoBehaviour
    {
        [SerializeField]
        private GameObject leftBird;
        [SerializeField]
        private float leftBirdSpeed;
        [SerializeField]
        private Vector2 leftBirdPos;
        [SerializeField]
        private GameObject rightBird;
        [SerializeField]
        private float rightBirdSpeed;
        [SerializeField]
        private Vector2 rightBirdPos;

        void Start()
        {
            GameObject lb = Instantiate(leftBird, new Vector3(leftBirdPos.x, leftBirdPos.y, 0), Quaternion.identity);
            lb.GetComponent<BirdMovement>().Speed = leftBirdSpeed;
            GameObject rb = Instantiate(rightBird, new Vector3(rightBirdPos.x, rightBirdPos.y, 0), Quaternion.identity);
            rb.GetComponent<BirdMovement>().Speed = rightBirdSpeed;
        }
    }
}
