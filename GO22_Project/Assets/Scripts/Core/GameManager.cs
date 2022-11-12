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
        [SerializeField]
        private List<GameConfig> gameConfigs;
        [SerializeField]
        private GameObject background;
        [SerializeField]
        private GameObject clicheHead;
        [SerializeField]
        private GameObject clicheTail;
        [SerializeField]
        private float gameDuration = 5f;
        [SerializeField]
        private float gameResultDuration = 1f;
        [SerializeField]
        private float transitionDuration = 0.5f;
        [SerializeField]
        private int forceGameIndex = -1;
        [SerializeField] Animator transition;

        // Singleton instance of GameManager
        public static GameManager Instance { get; private set; }
        // Player win event that other classes can subscribe to do something (ex: show happy face) when player wins
        public static event EventHandler playerWinEvent;
        public static event EventHandler playerLoseEvent;

        public static event EventHandler changeGameEvent;

        private SpriteRenderer backgroundImage;
        private TMP_Text clicheHeadText;
        private TMP_Text clicheTailText;
        // Game object instantiated for current game. Need to be destroyed at the end of each game
        private Stack<GameObject> charactersInGame = new Stack<GameObject>();
        private int currentGameIndex = 0;
        private GameResult gameResult;
        private int score;
        private int life;

        public void Win()
        {
            gameResult = GameResult.WIN;
            score++;
            GameConfig currentGame = gameConfigs[currentGameIndex];
            clicheHeadText.text = currentGame.ClicheHead;
            clicheTailText.text = currentGame.ClicheTail;
            playerWinEvent?.Invoke(this, EventArgs.Empty);
        }


        public void Lose() {
            gameResult = GameResult.LOSE;
            GameConfig currentGame = gameConfigs[currentGameIndex];
            clicheTailText.text = new Regex("[^\\s]").Replace(currentGame.ClicheTail,"?");
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
            backgroundImage = background.GetComponent<SpriteRenderer>();
            clicheHeadText = clicheHead.GetComponent<TMP_Text>();
            clicheTailText = clicheTail.GetComponent<TMP_Text>();
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
            backgroundImage.sprite = currentGame.BackgroundImage;
            clicheHeadText.text = $"{currentGame.ClicheHead}...";
            clicheTailText.text = "";
            currentGame.characters.ForEach(go => charactersInGame.Push(
                Instantiate(go.gameObject, new Vector3(go.x, go.y, 0), Quaternion.identity)));
        }

        void TransitionIn()
        {
            transition.SetBool("inTransition", true);
        }
        void TransitionOut()
        {
            transition.SetBool("inTransition", false);
        }

        void UnloadGame()
        {
            while (charactersInGame.Count > 0) {
                Destroy(charactersInGame.Pop());
            }
            backgroundImage.sprite = null;
            clicheHeadText.text = "";
            clicheTailText.text = "";
            changeGameEvent?.Invoke(this, EventArgs.Empty);
        }

        IEnumerator StartGamePlay()
        {
            while (true)
            {
                LoadGame();
                TransitionOut();
                yield return new WaitForSeconds(gameDuration);
                if (gameResult == GameResult.PRESTINE) {
                    Lose();
                    yield return new WaitForSeconds(gameResultDuration);
                }
                TransitionIn();
                yield return new WaitForSeconds(transitionDuration);
                UnloadGame();
            }
        }

        int chooseNextGameIndex()
        {
            if (forceGameIndex >= 0) {
                return forceGameIndex;
            }
            return UnityEngine.Random.Range(0, gameConfigs.Count);
        }
    }

    public enum GameResult {
        PRESTINE,
        WIN,
        LOSE
    }
}
