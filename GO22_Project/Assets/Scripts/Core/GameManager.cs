using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Namespace for TextMeshPro

namespace GO22
{
    public class GameManager : MonoBehaviour
    {
        private const string TRANSITION_TEXTURE = "_InputTexture";
        private const string TRANSITION_PROGRESS = "_Progress";
        public float gameDuration = 5f;
        [SerializeField]
        private List<GameConfig> gameConfigs;
        [SerializeField]
        private TMP_Text clicheHead;
        [SerializeField]
        private TMP_Text clicheTail;
        [SerializeField]
        private float gameResultDuration = 1f;
        [SerializeField]
        private float transitionDuration = 2f;
        [SerializeField]
        private Image transitionImage;
        [SerializeField]
        private List<Texture> transitionTextures;

        [SerializeField]
        private int forceGameIndex = -1;
        [SerializeField]
        private int initialLife = 4;
        [SerializeField]
        private float speedIncrement = 0.1f;
        [SerializeField]
        private ProgressBar progressBar;

        private Material transitionImageMaterial;

        // Singleton instance of GameManager
        public static GameManager Instance { get; private set; }
        // Player win event that other classes can subscribe to do something (ex: show happy face) when player wins
        public static event EventHandler playerWinEvent;
        public static event LoseEvent playerLoseEvent;
        public delegate void LoseEvent(int currentLife, int maxLife);
        public static event EventHandler startGameEvent;

        // Game object instantiated for current game. Need to be destroyed at the end of each game
        private Stack<GameObject> charactersInGame = new Stack<GameObject>();
        private int currentGameIndex = -1;
        private GameResult gameResult = GameResult.PRESTINE;
        private int life;
        private IEnumerator gamePlayCoroutine;
        private List<int> gameIndexToPick;
        private int gameRound;


        public void Win()
        {
            if (gameResult != GameResult.PRESTINE)
            {
                return;
            }

            gameResult = GameResult.WIN;
            AudioManager.Instance?.Play("Win");
            GameConfig currentGame = gameConfigs[currentGameIndex];
            clicheHead.text = currentGame.ClicheHead;
            clicheTail.text = currentGame.ClicheTail;
            playerWinEvent?.Invoke(this, EventArgs.Empty);
            ScoreManager.Instance?.IncrementScore();
        }

        public void Lose()
        {
            if (gameResult != GameResult.PRESTINE)
            {
                return;
            }

            gameResult = GameResult.LOSE;
            AudioManager.Instance?.Play("Lose");
            GameConfig currentGame = gameConfigs[currentGameIndex];
            clicheTail.text = new Regex("[^\\s]").Replace(currentGame.ClicheTail, "?");
            life--;
            playerLoseEvent?.Invoke(life, initialLife);

        }

        public void StartGamePlay()
        {
            transitionImageMaterial.SetFloat(TRANSITION_PROGRESS, 0);
            life = initialLife;
            currentGameIndex = -1;
            gameRound = 0;
            Time.timeScale = 1;
            gameIndexToPick = Enumerable.Range(0, gameConfigs.Count).ToList();
            ScoreManager.Instance?.ResetScore();
            gamePlayCoroutine = GameLoop();
            StartCoroutine(gamePlayCoroutine);
        }

        public void StopGamePlay()
        {
            transitionImageMaterial?.SetFloat(TRANSITION_PROGRESS, 0);
            Time.timeScale = 1;
            gameIndexToPick = null;
            if (gamePlayCoroutine != null)
            {
                StopCoroutine(gamePlayCoroutine);
            }
        }

        void Awake()
        {

            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        void Start()
        {
            transitionImageMaterial = transitionImage.material;
            StartGamePlay();
        }

        void OnDisable()
        {
            StopGamePlay();
        }

        void LoadNextGame()
        {
            if (gameConfigs.Count == 0)
            {
                return;
            }

            currentGameIndex = chooseNextGameIndex();
            gameResult = GameResult.PRESTINE;
            GameConfig currentGame = gameConfigs[currentGameIndex];
            clicheHead.text = $"{currentGame.ClicheHead}...";
            clicheTail.text = "";
            progressBar.ResetProgress();
            currentGame.characters.ForEach(go => charactersInGame.Push(
            Instantiate(go.gameObject, new Vector3(go.x, go.y, go.z), Quaternion.identity)));
        }

        void StartNextGame()
        {
            startGameEvent?.Invoke(this, EventArgs.Empty);
        }

        IEnumerator TransitionIn()
        {
            return Transition(1.5f, 0);
        }
        IEnumerator TransitionOut()
        {
            return Transition(0, 1.5f);
        }

        IEnumerator Transition(float start, float end)
        {
            if (transitionTextures.Count == 0)
            {
                yield break;
            }

            Texture texture = transitionTextures[UnityEngine.Random.Range(0, transitionTextures.Count)];
            transitionImageMaterial.SetTexture(TRANSITION_TEXTURE, texture);
            float timeElapsed = 0;
            while (timeElapsed < transitionDuration)
            {
                float progress = Mathf.Lerp(start, end, timeElapsed / transitionDuration);
                transitionImageMaterial.SetFloat(TRANSITION_PROGRESS, progress);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            transitionImageMaterial.SetFloat(TRANSITION_PROGRESS, end);
        }

        void UnloadGame()
        {
            while (charactersInGame.Count > 0)
            {
                Destroy(charactersInGame.Pop());
            }
            clicheHead.text = "";
            clicheTail.text = "";
        }

        void endGame()
        {
            StopGamePlay();
            SceneManager.Instance?.GoToEnd();
        }

        IEnumerator GameLoop()
        {
            while (life > 0)
            {
                LoadNextGame();
                yield return TransitionOut();
                StartNextGame();
                float gameTimePassed = 0;
                float gameDuration = gameConfigs[currentGameIndex].gameDuration;
                yield return new WaitUntil(() =>
                {
                    gameTimePassed += Time.deltaTime;
                    progressBar.UpdateProgress(gameTimePassed, gameDuration);
                    return gameTimePassed >= gameDuration ||
                        gameResult != GameResult.PRESTINE;
                });
                if (gameResult == GameResult.PRESTINE)
                {
                    Lose();
                }
                yield return new WaitForSeconds(gameResultDuration);
                yield return TransitionIn();
                UnloadGame();
            }
            endGame();
        }

        int chooseNextGameIndex()
        {
            if (forceGameIndex >= 0)
            {
                return forceGameIndex;
            }
            if (gameIndexToPick == null || gameIndexToPick.Count == 0)
            {
                gameIndexToPick = Enumerable.Range(0, gameConfigs.Count).ToList();
                Time.timeScale = 1f + speedIncrement * (float)++gameRound; ;
            }
            int nextIndex = chooseNextGameIndexDifferentFromCurrent(10);
            int nextGameIndex = gameIndexToPick[nextIndex];
            gameIndexToPick.RemoveAt(nextIndex);
            return nextGameIndex;
        }

        int chooseNextGameIndexDifferentFromCurrent(int maxRetry)
        {
            int nextIndex;
            int retryCount = 0;
            do
            {
                nextIndex = UnityEngine.Random.Range(0, gameIndexToPick.Count);
            } while (gameIndexToPick[nextIndex] == currentGameIndex && retryCount++ < maxRetry);
            return nextIndex;
        }
    }

    public enum GameResult
    {
        PRESTINE,
        WIN,
        LOSE
    }
}
