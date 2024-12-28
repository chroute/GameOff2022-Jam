using UnityEngine;

namespace GO22
{
    public class WordBlockerMovement : MonoBehaviour
    {

        [SerializeField]
        private float moveSpeed = 2f;  // Controls how fast the oscillation occurs
        
        [SerializeField]
        private float moveDistance = 100f;  // Controls how far up/down the blocker moves
        
        private RectTransform rectTransform;
        private float panelHeight;
        private float startingY;
        private float timePassed;

        void Start()
        {
            rectTransform = GetComponent<RectTransform>();
            panelHeight = rectTransform.rect.height;
            startingY = rectTransform.anchoredPosition.y;  // Store the initial Y position
        }

        void Update()
        {
            AutoMoveTextBlocker();
        }

        void AutoMoveTextBlocker()
        {
            // Increment time
            timePassed += Time.deltaTime;
            
            // Calculate new Y position using sine wave, but scale the amplitude by time scale
            float scaledDistance = moveDistance * Time.timeScale;
            float newY = startingY + Mathf.Sin(timePassed * moveSpeed * Time.timeScale) * scaledDistance;
                        
            // Create new position vector
            Vector3 newPos = rectTransform.anchoredPosition;
            newPos.y = newY;
            
            // Clamp the position to stay within panel bounds
            newPos.y = Mathf.Clamp(newPos.y, -panelHeight / 2, panelHeight / 2);
            
            // Apply the new position
            rectTransform.anchoredPosition = newPos;
        }
    }
}
