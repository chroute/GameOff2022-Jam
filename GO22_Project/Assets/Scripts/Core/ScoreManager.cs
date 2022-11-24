using UnityEngine;

namespace GO22
{
    public class ScoreManager : MonoBehaviour
    {
        public static ScoreManager Instance { get; private set; }
        public int Score { get; private set; }


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

        public void IncrementScore() {
            Score++;
        }

        public void ResetScore() {
            Score = 0;
        }
    }
}
