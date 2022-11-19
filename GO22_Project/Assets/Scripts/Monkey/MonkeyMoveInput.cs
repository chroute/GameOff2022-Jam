using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

namespace GO22
{
    public class MonkeyMoveInput : MonoBehaviour
    {
        private MonkeyMovement monkeyMovement;

        private bool ready;
        private List<int> targetMoves;
        private List<int> moves = new List<int>();


        public void StartMimick(List<int> moves)
        {
            if (moves != null)
            {
                ready = true;
                targetMoves = moves;
            }
        }

        void OnEnable() {
            MonkeyMoveInitializer.OnMovesComplete += StartMimick;
        }

        void OnDisable() {
            MonkeyMoveInitializer.OnMovesComplete -= StartMimick;
        }

        void Awake()
        {
            monkeyMovement = GetComponent<MonkeyMovement>();
        }

        void OnMove(InputValue input)
        {
            if (!ready)
            {
                return;
            }

            Vector2 inputValue = input.Get<Vector2>();
            int currentMove = -1;
            if (inputValue == Vector2.left)
            {
                currentMove = 0;
            }
            else if (inputValue == Vector2.up)
            {
                currentMove = 1;
            }
            else if (inputValue == Vector2.right)
            {
                currentMove = 2;
            }
            else if (inputValue == Vector2.down)
            {
                currentMove = 3;
            }
            if (currentMove < 0)
            {
                monkeyMovement.Idle();
            }
            else
            {
                monkeyMovement.Move(currentMove);
                moves.Add(currentMove);
            }
            checkResult();
        }

        void checkResult()
        {
            int movesCount = moves.Count;
            if (movesCount < 1 || movesCount > targetMoves.Count)
            {
                return;
            }
            int latestMove = movesCount - 1;

            if (moves[latestMove] != targetMoves[latestMove])
            {
                GameManager.Instance?.Lose();
            }
            else if (movesCount == targetMoves.Count)
            {
                GameManager.Instance?.Win();
            }
        }
    }
}
