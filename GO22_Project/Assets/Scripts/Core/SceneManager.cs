using UnityEngine;

namespace GO22
{
    public class SceneManager : MonoBehaviour
    {
        public static SceneManager Instance { get; private set; }


        void Awake()
        {

            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
        }

        public void GoToGamePlay() {
            UnityEngine.SceneManagement.SceneManager.LoadScene("GamePlay");
        }

        public void GoToEnd() {
            UnityEngine.SceneManagement.SceneManager.LoadScene("End");
        }
    }
}
