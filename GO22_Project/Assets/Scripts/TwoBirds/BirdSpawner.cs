using System.Collections;
using UnityEngine;

namespace GO22
{
    public class BirdSpawner : MonoBehaviour
    {
        [SerializeField]
        private GameObject leftBird;
        [SerializeField]
        private Vector2 leftBirdPos;
        [SerializeField]
        private GameObject rightBird;
        [SerializeField]
        private Vector2 rightBirdPos;
        [SerializeField]
        private float maxBirdSpeed;
        [SerializeField]
        private float minBirdSpeed;

        private float spawnInterval = 2f;

        void Start()
        {
            StartCoroutine(SpawnBirds());
        }

        void NewBirds()
        {
            GameObject lb = Instantiate(leftBird, new Vector3(leftBirdPos.x, leftBirdPos.y, 0), Quaternion.identity);
            lb.GetComponent<BirdMovement>().Speed = Random.Range(minBirdSpeed, maxBirdSpeed);
            GameObject rb = Instantiate(rightBird, new Vector3(rightBirdPos.x, rightBirdPos.y, 0), Quaternion.identity);
            rb.GetComponent<BirdMovement>().Speed = -Random.Range(minBirdSpeed, maxBirdSpeed);
        }

        IEnumerator SpawnBirds()
        {
            while (true)
            {
                NewBirds();
                yield return new WaitForSeconds(spawnInterval);
            }
        }
    }
}
