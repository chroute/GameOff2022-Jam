using UnityEngine;

namespace GO22
{
    public class PlatformWidth : MonoBehaviour
    {
        public static PlatformWidth Instance { get; private set; }

        private SpriteRenderer spriteRenderer;
        public float Width { get; private set; }
        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
                spriteRenderer = GetComponent<SpriteRenderer>();
                Width = spriteRenderer.bounds.size.x;
            }
        }
    }
}
