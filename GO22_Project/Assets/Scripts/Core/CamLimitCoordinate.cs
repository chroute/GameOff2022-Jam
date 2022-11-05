using UnityEngine;

namespace GO22
{
    public class CamLimitCoordinate : MonoBehaviour
    {
        public static CamLimitCoordinate Instance {get; private set;}

        public float MaxX {get; private set;}
        public float MinX {get; private set;}
        public float MaxY {get; private set;}
        public float MinY {get; private set;}

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Vector3 topRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));
                Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
                MaxX = topRight.x;
                MinX = bottomLeft.x;
                MaxY = topRight.y;
                MinY = bottomLeft.y;
                Instance = this;
            }
        }
    }
}
