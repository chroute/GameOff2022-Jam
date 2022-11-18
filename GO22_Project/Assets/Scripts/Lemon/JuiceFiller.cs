using UnityEngine;

namespace GO22
{
    public class JuiceFiller : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;
        private Material material;

        void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            material = spriteRenderer.material;
            material.SetFloat("_Progress", 0);
        }

        public void FillBottle(int max, int current)
        {
            material.SetFloat("_Progress", Mathf.Lerp(0, 1, (float)current / max));
        }
    }
}
