using UnityEngine;

namespace GO22
{
    public class CamFollow : MonoBehaviour
    {
        [SerializeField]
        private float smoothFactor = 0.5f;
        private Transform target;
        private Vector3 initialPosition;
        
        public void FollowMe(Transform targetObject)
        {
            initialPosition = transform.position;
            target = targetObject;
        }

        public void unFollow()
        {
            target = null;
            transform.position = initialPosition;
        }

        private void LateUpdate()
        {
            if (target != null)
            {
                Vector3 targetPos = new Vector3(target.position.x,
                        target.position.y,
                        transform.position.z);
                transform.position = Vector3.Lerp(transform.position, targetPos, smoothFactor);
            }
        }
    }
}
