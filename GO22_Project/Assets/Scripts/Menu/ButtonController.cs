using UnityEngine;

namespace GO22
{
    public class ButtonController : MonoBehaviour
    {

        private void Update() {
            if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.E))
            {
                PlayGame();
            }
        }

        public void PlayGame() {
            SceneManager.Instance.GoToGamePlay();
        }
    }
}
