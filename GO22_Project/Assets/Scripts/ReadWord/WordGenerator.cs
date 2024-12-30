using UnityEngine;

namespace GO22
{
    public class WordGenerator
    {
        private static string[] wordList = new string[] { "Dog", "Cow", "Cat", "Horse", "Donkey", "Tiger", "Lion", "Panther", "Leopard", "Bear", "Elephant", "Crocodile", "Ostrich", "Chimpanzee", "Giraffe", "Chameleon","Jellyfish","Flamingo", "Whale", "Turtle" };
        private static WordGenerator instance;
        public string CurrentWord { get; private set; }

        private WordGenerator()
        {
        }

        public string PickWord()
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
