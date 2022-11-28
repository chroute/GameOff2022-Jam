using System.Collections.Generic;
using UnityEngine;

namespace GO22
{
    public class MessengerGameControl : MonoBehaviour
    {
        [SerializeField]
        private Sprite bandit;

        [SerializeField]
        private Sprite messenger;
        [SerializeField]
        private GameObject card;
        [SerializeField]
        private int numberOfCards = 3;
        [SerializeField]
        private int xIncrement = 3;
        [SerializeField]
        private int initialX = -3;

        private int messengerIndex;
        private HashSet<int> banditLeft = new HashSet<int>();
        private List<GameObject> createdCards = new List<GameObject>();

        void Start()
        {
            messengerIndex = UnityEngine.Random.Range(0, numberOfCards);
            int i = 0;
            while (i < numberOfCards)
            {
                GameObject createdCard = Instantiate(card, new Vector2(initialX + xIncrement * i, 1), Quaternion.identity);
                SpriteRenderer spriteRenderer = createdCard.transform.GetChild(0).GetComponent<SpriteRenderer>();
                if (i == messengerIndex)
                {
                    spriteRenderer.sprite = messenger;
                }
                else
                {
                    spriteRenderer.sprite = bandit;
                    banditLeft.Add(i);
                }
                createdCards.Add(createdCard);
                i++;
            }
        }

        void OnDisable()
        {
            foreach (GameObject go in createdCards)
            {
                Destroy(go);
            }
        }

        public void FireAtPosition(int pos)
        {
            if (banditLeft.Count == 0)
            {
                return;
            }
            int index = pos + 1;
            CardTurn cardTurn = createdCards[index].GetComponent<CardTurn>();
            if (!cardTurn.FlashCardEnded)
            {
                return;
            }

            if (index == messengerIndex)
            {
                cardTurn.ShowCard(() => cardTurn.TiltCard());
                GameManager.Instance?.Lose();
                return;
            }
            bool shotNewBandit = banditLeft.Remove(index);
            if (!shotNewBandit)
            {
                return;
            }

            if (banditLeft.Count == 0)
            {
                cardTurn.ShowCard(() =>
                    createdCards[messengerIndex].GetComponent<CardTurn>().ShowCard(() =>
                        GameManager.Instance?.Win()));
            }
            else
            {
                cardTurn.ShowCard(null);
            }
        }
    }
}
