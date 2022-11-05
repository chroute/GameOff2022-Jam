using System;
using UnityEngine;

namespace GO22
{
    // Class to extend to destroy the game object when current game unloads
    public class DestroyAfterGame : MonoBehaviour
    {
        void OnEnable() {
            GameManager.changeGameEvent+= DestroySelf;
        }

        void OnDisable() {
            GameManager.changeGameEvent-= DestroySelf;
        }

        void DestroySelf(object sender, EventArgs eventArgs) {
            Destroy(this.gameObject);
        }
    }
}
