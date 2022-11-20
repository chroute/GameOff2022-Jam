using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GO22
{
    public class LemonSpawner : MonoBehaviour
    {
        [SerializeField]
        private GameObject lemon;
        [SerializeField]
        private GameObject stone;
        [SerializeField]
        private float force = 2.5f;
        [SerializeField]
        private int targetLemon = 5;
        private float spawnInterval;
        private IEnumerator lemonCoroutine;
        private IEnumerator stoneCoroutine;

        private Stack<GameObject> spawnedObjects = new Stack<GameObject>();
        void Start()
        {
            spawnInterval = (float) GameManager.Instance.gameDuration / (targetLemon + 3);
            lemonCoroutine = SpawnLemons();
            stoneCoroutine = SpawnStones();
            StartCoroutine(lemonCoroutine);
            StartCoroutine(stoneCoroutine);
        }

        void OnDisable()
        {
            StopCoroutine(lemonCoroutine);
            StopCoroutine(stoneCoroutine);
            while (spawnedObjects.Count > 0)
            {
                Destroy(spawnedObjects.Pop());
            }
        }

        IEnumerator SpawnLemons()
        {
            while (true)
            {
                SpawnNew(lemon);
                yield return new WaitForSeconds(spawnInterval);
            }
        }

        IEnumerator SpawnStones()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(1f, 3f));
                SpawnNew(stone);
            }
        }

        void SpawnNew(GameObject objectToCreate)
        {
            float x = Random.Range(CamLimitCoordinate.Instance.MinX + 1, CamLimitCoordinate.Instance.MaxX - 1);
            GameObject newObject = Instantiate(objectToCreate, new Vector3(x, CamLimitCoordinate.Instance.MaxY, 0), Quaternion.identity);
            Rigidbody2D rb = newObject.GetComponent<Rigidbody2D>();
            rb.AddForce(new Vector2(Random.Range(-force, force), 0), ForceMode2D.Impulse);
            spawnedObjects.Push(newObject);
        }
    }
}
