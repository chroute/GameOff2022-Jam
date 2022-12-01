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
        private Image instruction;
        [SerializeField]
        private TMP_Text clicheHead;
        [SerializeField]
        private TMP_Text clicheTail;
        [SerializeField]
        private float gameResultDuration = 1f;
        [SerializeField]
        private float transitionDuration = 2f;
        [SerializeField]
        private float countDownDuration = 1f;
        [SerializeField]
        private Image transitionImage;
        [SerializeField]
        private List<Texture> transitionTextures;
        [SerializeField]
        private Image countDownImage;
        [SerializeField]
        private int forceGameIndex = -1;
        [SerializeField]
        private int initialLife = 5;
        [SerializeField]
        private float speedIncrement = 0.1f;
        [SerializeField]
        private float pitchIncrement = 0.1f;
        [SerializeField]
        private ProgressBar progressBar;

        private Material transitionImageMaterial;
        private Material countDownImageMaterial;

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
            life = initialLife;
            currentGameIndex = -1;
            gameRound = 0;
            Time.timeScale = 1;
            gameIndexToPick = Enumerable.Range(0, gameConfigs.Count).ToList();
            ScoreManager.Instance?.ResetScore();
            ResetGame();
            gamePlayCoroutine = GameLoop();
            StartCoroutine(gamePlayCoroutine);
        }

        public void StopGamePlay()
        {
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
            countDownImageMaterial = countDownImage.material;
            StartGamePlay();
        }

        void OnDisable()
        {
            StopGamePlay();
        }

        void ResetGame()
        {
            clicheHead.text = "";
            clicheTail.text = "";
            instruction.sprite = null;
            instruction.color = Color.black;
            progressBar.ResetProgress();
            gameResult = GameResult.PRESTINE;
            transitionImageMaterial.SetFloat(TRANSITION_PROGRESS, 0);
            countDownImageMaterial.SetFloat(TRANSITION_PROGRESS, 0);
        }

        void LoadNextGame()
        {
            if (gameConfigs.Count == 0)
            {
                return;
            }

            currentGameIndex = chooseNextGameIndex();
            GameConfig currentGame = gameConfigs[currentGameIndex];
            clicheHead.text = $"{currentGame.ClicheHead}...";
            clicheTail.text = "";
            instruction.sprite = currentGame.Instruction;
            instruction.color = Color.white;
            currentGame.characters.ForEach(go => charactersInGame.Push(Instantiate(go.gameObject, new Vector3(go.x, go.y, go.z), Quaternion.identity)));
            GameObject background = InitializeBackgroundWithPitch(currentGame.background);
            if (background != null)
            {
                charactersInGame.Push(background);
            }
        }

        GameObject InitializeBackgroundWithPitch(GameObjectWithPosition background)
        {
            GameObject createdObject = null;
            if (background != null && background.gameObject != null)
            {
                createdObject = Instantiate(background.gameObject, new Vector3(background.x, background.y, background.z), Quaternion.identity);
                AudioSource audioSource = createdObject.GetComponent<AudioSource>();
                if (audioSource != null)
                {
                    audioSource.pitch += pitchIncrement * gameRound;
                }
            }
            return createdObject;
        }

        void StartNextGame()
        {
            startGameEvent?.Invoke(this, EventArgs.Empty);
        }

        IEnumerator CountDown()
        {
            countDownImage.enabled = true;
            TMP_Text countDown = countDownImage.GetComponentInChildren<TMP_Text>();
            countDown.text = "3";
            AudioManager.Instance?.Play("Beep");
            yield return Transition(countDownImageMaterial, 0f, 1.5f, countDownDuration);
            countDown.text = "2";
            AudioManager.Instance?.Play("Beep");
            yield return Transition(countDownImageMaterial, 0f, 1.5f, countDownDuration);
            countDown.text = "1";
            AudioManager.Instance?.Play("Beep");
            yield return Transition(countDownImageMaterial, 0f, 1.5f, countDownDuration);
            countDown.text = "";
            AudioManager.Instance?.Play("BeepEnd");
            countDownImage.enabled = false;
        }

        IEnumerator TransitionIn()
        {
            Texture texture = transitionTextures[UnityEngine.Random.Range(0, transitionTextures.Count)];
            transitionImageMaterial.SetTexture(TRANSITION_TEXTURE, texture);
            return Transition(transitionImageMaterial, 1.5f, 0, transitionDuration);
        }

        IEnumerator TransitionOut()
        {
            Texture texture = transitionTextures[UnityEngine.Random.Range(0, transitionTextures.Count)];
            transitionImageMaterial.SetTexture(TRANSITION_TEXTURE, texture);
            return Transition(transitionImageMaterial, 0, 1.5f, transitionDuration);
        }

        IEnumerator Transition(Material material, float start, float end, float duration)
        {
            float timeElapsed = 0;
            while (timeElapsed < duration)
            {
                float progress = Mathf.Lerp(start, end, timeElapsed / duration);
                material.SetFloat(TRANSITION_PROGRESS, progress);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            material.SetFloat(TRANSITION_PROGRESS, end);
        }

        void UnloadGame()
        {
            while (charactersInGame.Count > 0)
            {
                Destroy(charactersInGame.Pop());
            }
            ResetGame();
        }

        void endGame()
        {
            StopGamePlay();
            SceneManager.Instance?.GoToEnd();
        }

        IEnumerator GameLoop()
        {
            yield return CountDown();
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
                accelerateGame();
                return forceGameIndex;
            }
            if (gameIndexToPick == null || gameIndexToPick.Count == 0)
            {
                gameIndexToPick = Enumerable.Range(0, gameConfigs.Count).ToList();
                accelerateGame();
            }
            int nextIndex = chooseNextGameIndexDifferentFromCurrent(10);
            int nextGameIndex = gameIndexToPick[nextIndex];
            gameIndexToPick.RemoveAt(nextIndex);
            return nextGameIndex;
        }

        void accelerateGame()
        {
            Time.timeScale = 1f + speedIncrement * (float)++gameRound;
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
