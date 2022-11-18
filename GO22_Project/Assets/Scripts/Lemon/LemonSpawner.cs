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
        private float force = 2;
        [SerializeField]
        private float spawnInterval = 0.5f;
        private IEnumerator coroutine;

        private Stack<GameObject> spawnedLemons = new Stack<GameObject>();
        // Start is called before the first frame update
        void Start()
        {
            coroutine = SpawnLemeons();
            StartCoroutine(coroutine);
        }

        void OnDisable()
        {
            StopCoroutine(coroutine);
            while (spawnedLemons.Count > 0)
            {
                Destroy(spawnedLemons.Pop());
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        IEnumerator SpawnLemeons()
        {
            while (true)
            {
                NewLemon();
                yield return new WaitForSeconds(spawnInterval);
            }
        }

        void NewLemon()
        {
            float x = Random.Range(CamLimitCoordinate.Instance.MinX + 1, CamLimitCoordinate.Instance.MaxX - 1);
            GameObject newLemon = Instantiate(lemon, new Vector3(x, CamLimitCoordinate.Instance.MaxY, 0), Quaternion.identity);
            Rigidbody2D rb = newLemon.GetComponent<Rigidbody2D>();
            rb.AddForce(new Vector2(Random.Range(-force, force), 0), ForceMode2D.Impulse);
            spawnedLemons.Push(newLemon);
        }
    }
}
