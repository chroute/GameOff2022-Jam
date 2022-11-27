using System.Collections.Generic;
using UnityEngine;

namespace GO22
{
    public class WormSpawner : MonoBehaviour
    {

        [SerializeField]
        private GameObject worm;
        [SerializeField]
        private int wormsToSpawn;
        private Stack<GameObject> spawnedObjects = new Stack<GameObject>();
        void Start()
        {
            int i = 0;
            while (i++ < wormsToSpawn)
            {
                Vector2 position = new Vector2(
                    Random.Range(CamLimitCoordinate.Instance.MinX + 2, CamLimitCoordinate.Instance.MaxX - 2),
                    Random.Range(CamLimitCoordinate.Instance.MinY + 2, CamLimitCoordinate.Instance.MaxY - 2));
                spawnedObjects.Push(Instantiate(worm, position, Quaternion.identity));
            }
        }

        void OnDisable()
        {
            while (spawnedObjects.Count > 0)
            {
                Destroy(spawnedObjects.Pop());
            }
        }
    }
}
