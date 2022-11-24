using UnityEngine;

namespace GO22
{
    public class ButtonController : MonoBehaviour
    {
        public void PlayGame() {
            SceneManager.Instance.GoToGamePlay();
        }
    }
}
