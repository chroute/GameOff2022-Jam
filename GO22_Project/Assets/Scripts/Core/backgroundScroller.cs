using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GO22
{
    
    public class backgroundScroller : MonoBehaviour
    {
    [SerializeField] Vector2 moveSpeed;
    Vector2 offset;
    Material material;

        void Awake()
        {
            material = GetComponent<SpriteRenderer>().material;
            offset.x = material.mainTextureOffset.x;
        }

        void Update()
        {
            offset = moveSpeed * Time.deltaTime;
            material.mainTextureOffset += offset;       
        }
    }
}
