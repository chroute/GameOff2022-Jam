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
        private int forceGameIndex = -1;
        [SerializeField]
        private Image transitionImage;
        [SerializeField]
        private List<Texture> transitionTextures;
        private Material transitionImageMaterial;

        // Singleton instance of GameManager
        public static GameManager Instance { get; private set; }
        // Player win event that other classes can subscribe to do something (ex: show happy face) when player wins
        public static event EventHandler playerWinEvent;
        public static event EventHandler playerLoseEvent;

        public static event EventHandler changeGameEvent;
        private GameObject mainCamera;
        [SerializeField] public Vector3 cameraInitialPosition;
        private SpriteRenderer backgroundImage;
        private TMP_Text clicheHeadText;
        private TMP_Text clicheTailText;
        // Game object instantiated for current game. Need to be destroyed at the end of each game
        private Stack<GameObject> charactersInGame = new Stack<GameObject>();
        private int currentGameIndex = 0;
        private GameResult gameResult;
        private int score;
        private int life;
        private const string CAMERA = "MainCamera";


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
            mainCamera = GameObject.FindGameObjectsWithTag(CAMERA)[0];
            cameraInitialPosition = mainCamera.transform.position;

        }

        void Start()
        {
            transitionImageMaterial = transitionImage.material;
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
            clicheHead.text = $"{currentGame.ClicheHead}...";
            clicheTail.text = "";
            currentGame.characters.ForEach(go => charactersInGame.Push(
            Instantiate(go.gameObject, new Vector3(go.x, go.y, go.z), Quaternion.identity)));
        }

        IEnumerator TransitionIn()
        {
            return Transition(1, 0);
        }
        IEnumerator TransitionOut()
        {
            return Transition(0, 1);
        }

        IEnumerator Transition(float startAlpha, float endAlpha)
        {
            if (transitionTextures.Count == 0) {
                yield break;
            }

            Texture texture = transitionTextures[UnityEngine.Random.Range(0, transitionTextures.Count)];
            transitionImageMaterial.SetTexture("_InputTexture", texture);
            float timeElapsed = 0;
            while (timeElapsed < transitionDuration)
            {
                float progress = Mathf.Lerp(startAlpha, endAlpha, timeElapsed / transitionDuration);
                transitionImageMaterial.SetFloat("_Progress", progress);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
                transitionImageMaterial.SetFloat("_Progress", 1);
        }

        void UnloadGame()
        {
            while (charactersInGame.Count > 0)
            {
                Destroy(charactersInGame.Pop());
            }
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

        private void ResetCamera()
        {
            mainCamera.transform.position = cameraInitialPosition;
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
