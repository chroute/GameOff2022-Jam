using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace GO22
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private List<GameConfig> gameConfigs;
        [SerializeField]
        private GameObject background;
        [SerializeField]
        private GameObject cliche;
        [SerializeField]
        private float gameDuration = 5f;
        [SerializeField]
        private float gameEndingDuration = 2f;

        // Singleton instance of GameManager
        public static GameManager Instance { get; set; }
        // Player win event that other classes can subscribe to do something (ex: show happy face) when player wins
        public static event EventHandler playerWinEvent;

        private Image backgroundImage;
        private Text clicheText;
        private int currentGameIndex = 0;
        // Game object instantiated for current game. Need to be destroyed at the end of each game
        private List<GameObject> charactersInGame = new List<GameObject>();
        private bool win;
        private int score;
        private int life;

        public void Win() {
            win = true;
            playerWinEvent?.Invoke(this, EventArgs.Empty);
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
            backgroundImage = background.GetComponent<Image>();
            clicheText = cliche.GetComponent<Text>();
            StartCoroutine(StartGamePlay());
        }

        void LoadGame()
        {
            if (gameConfigs.Count == 0) {
                return;
            }

            currentGameIndex = chooseNextGameIndex();
            win = false;
            GameConfig currentGame = gameConfigs[currentGameIndex];
            backgroundImage.sprite = currentGame.BackgroundImage;
            clicheText.text = $"{currentGame.ClicheHead}...";
            currentGame.characters.ForEach(go => charactersInGame.Add(Instantiate(go)));
        }

        void GameEnding() {
            GameConfig currentGame = gameConfigs[currentGameIndex];
            clicheText.text = $"{currentGame.ClicheHead} {currentGame.ClicheTail}";
            if (!win) {
                // TODO: character sad face
            }
        }

        void UnloadGame()
        {
            charactersInGame.ForEach(go => Destroy(go));
        }

        IEnumerator StartGamePlay() {
            while (true) {
                UnloadGame();
                LoadGame();
                yield return new WaitForSeconds(gameDuration);
                GameEnding();
                yield return new WaitForSeconds(gameEndingDuration);
            }
        }

        int chooseNextGameIndex() {
            return UnityEngine.Random.Range(0, gameConfigs.Count);
        }
    }
}
