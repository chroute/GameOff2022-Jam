using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GO22
{
    public class MonkeyMoveInitializer : MonoBehaviour
    {
        [SerializeField]
        private int numberOfMoves = 5;
        [SerializeField]
        private GameObject mimickMonkey;

        public  delegate void MovesComplete(List<int> moves);
        public static event MovesComplete OnMovesComplete;

        private MonkeyMovement monkeyMovement;
        private float secPerMove;
        
        private int moveCount = 0;
        private List<int> moves = new List<int>();

        void Start()
        {
            float? gameDuration = GameManager.Instance?.gameDuration;
            secPerMove = (gameDuration ?? 5) / (3 * numberOfMoves);
            monkeyMovement = GetComponent<MonkeyMovement>();
            StartCoroutine(StartMoving());
        }

        void NextMove()
        {
            int move = UnityEngine.Random.Range(0, 4);
            moves.Add(move);
            monkeyMovement.Move(move);
        }

        IEnumerator StartMoving()
        {
            if (moveCount == 0)
            {
                yield return new WaitForSeconds(secPerMove / 2);
            }

            while (moveCount++ < numberOfMoves)
            {
                NextMove();
                yield return new WaitForSeconds(secPerMove);
                monkeyMovement.Idle();
                yield return new WaitForSeconds(secPerMove / 2);
                if (moveCount == numberOfMoves) {
                    OnMovesComplete?.Invoke(moves);
                }
            }
        }
    }
}


