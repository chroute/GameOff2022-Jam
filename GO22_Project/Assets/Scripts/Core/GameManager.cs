using System.Collections.Generic;
using System.Collections;
using System;
using System.Text.RegularExpressions;
using UnityEngine;
using TMPro; // Namespace for TextMeshPro

namespace GO22
{
    public class GameManager : MonoBehaviour
    {
        public float gameDuration = 5f;
        [SerializeField]
        private List<GameConfig> gameConfigs;
        [SerializeField]
        private SpriteRenderer background;
        [SerializeField]
        private TMP_Text clicheHead;
        [SerializeField]
        private TMP_Text clicheTail;
        [SerializeField]
        private float gameResultDuration = 1f;
        [SerializeField]
        private float transitionDuration = 2f;
        [SerializeField]
        private int forceGameIndex = -1;
        [SerializeField]
        private CanvasRenderer transitionImage;

        // Singleton instance of GameManager
        public static GameManager Instance { get; private set; }
        // Player win event that other classes can subscribe to do something (ex: show happy face) when player wins
        public static event EventHandler playerWinEvent;
        public static event EventHandler playerLoseEvent;

        public static event EventHandler changeGameEvent;

        // Game object instantiated for current game. Need to be destroyed at the end of each game
        private Stack<GameObject> charactersInGame = new Stack<GameObject>();
        private int currentGameIndex = 0;
        private GameResult gameResult;
        private int score;
        private int life;

        public void Win()
        {
            if (gameResult != GameResult.PRESTINE) {
                return;
            }

            gameResult = GameResult.WIN;
            score++;
            GameConfig currentGame = gameConfigs[currentGameIndex];
            clicheHead.text = currentGame.ClicheHead;
            clicheTail.text = currentGame.ClicheTail;
            playerWinEvent?.Invoke(this, EventArgs.Empty);
        }

        public void Lose() {
            if (gameResult != GameResult.PRESTINE) {
                return;
            }

            gameResult = GameResult.LOSE;
            GameConfig currentGame = gameConfigs[currentGameIndex];
            clicheTail.text = new Regex("[^\\s]").Replace(currentGame.ClicheTail, "?");
            playerLoseEvent?.Invoke(this, EventArgs.Empty);
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
                DontDestroyOnLoad(this.gameObject);
            }
        }

        void Start()
        {
            StartCoroutine(StartGamePlay());
        }

        void LoadGame()
        {
            if (gameConfigs.Count == 0)
            {
                return;
            }

            currentGameIndex = chooseNextGameIndex();
            gameResult = GameResult.PRESTINE;
            GameConfig currentGame = gameConfigs[currentGameIndex];
            background.sprite = currentGame.BackgroundImage;
            clicheHead.text = $"{currentGame.ClicheHead}...";
            clicheTail.text = "";
            currentGame.characters.ForEach(go => charactersInGame.Push(
                Instantiate(go.gameObject, new Vector3(go.x, go.y, 0), Quaternion.identity)));
        }

        IEnumerator TransitionIn()
        {
            return Transition(0, 1);
        }
        IEnumerator TransitionOut()
        {
            return Transition(1, 0);
        }

        IEnumerator Transition(float startAlpha, float endAlpha)
        {
            float timeElapsed = 0;
            while (timeElapsed < transitionDuration)
            {
                transitionImage.SetAlpha(Mathf.Lerp(startAlpha, endAlpha, timeElapsed / transitionDuration));
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            transitionImage.SetAlpha(endAlpha);
        }

        void UnloadGame()
        {
            while (charactersInGame.Count > 0)
            {
                Destroy(charactersInGame.Pop());
            }
            background.sprite = null;
            clicheHead.text = "";
            clicheTail.text = "";
            changeGameEvent?.Invoke(this, EventArgs.Empty);
        }

        IEnumerator StartGamePlay()
        {
            while (true)
            {
                LoadGame();
                yield return TransitionOut();
                yield return new WaitForSeconds(gameDuration);
                if (gameResult == GameResult.PRESTINE)
                {
                    Lose();
                    yield return new WaitForSeconds(gameResultDuration);
                }
                yield return TransitionIn();
                UnloadGame();
            }
        }

        int chooseNextGameIndex()
        {
            if (forceGameIndex >= 0)
            {
                return forceGameIndex;
            }
            return UnityEngine.Random.Range(0, gameConfigs.Count);
        }
    }

    public enum GameResult
    {
        PRESTINE,
        WIN,
        LOSE
    }
}
