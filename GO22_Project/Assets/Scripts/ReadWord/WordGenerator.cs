using UnityEngine;

namespace GO22
{
    public class WordGenerator
    {
        private static string[] wordList = new string[] { "Dog", "Cow", "Cat", "Horse", "Donkey", "Tiger", "Lion", "Panther", "Leopard", "Bear", "Elephant" };
        private static WordGenerator instance;
        public string CurrentWord { get; private set; }

        public string LoadWord()
        {
            CurrentWord = pickWord();
            return CurrentWord;
        }

        public void UnLoadWord()
        {
            CurrentWord = null;
        }

        public bool IsWordLoaded() {
            return CurrentWord != null;
        }

        private WordGenerator()
        {
        }


        private string pickWord()
        {
            return wordList[Random.Range(0, wordList.Length - 1)];
        }

        public static WordGenerator Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new WordGenerator();
                }
                return instance;
            }
        }

    }
}
