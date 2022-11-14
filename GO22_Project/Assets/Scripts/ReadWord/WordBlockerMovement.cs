using UnityEngine;

namespace GO22
{
    public class WordBlockerMovement : MonoBehaviour
    {
        [SerializeField]
        private float moveSpeed = 20;
        private RectTransform rectTransform;
        private float panelHeight;

        void Start()
        {
            rectTransform = GetComponent<RectTransform>();
            panelHeight = rectTransform.rect.height;
        }

        void Update()
        {
            moveTextBlocker();
        }

        void moveTextBlocker()
        {
            Vector3 curPos = rectTransform.anchoredPosition;
            if (Input.GetKey(KeyCode.UpArrow))
            {
                curPos.y += moveSpeed * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                curPos.y -= moveSpeed * Time.deltaTime;
            }
            curPos.y = Mathf.Clamp(curPos.y, -panelHeight / 2, panelHeight / 2);
            rectTransform.anchoredPosition = curPos;
        }
    }
}
