using UnityEngine;

namespace GO22
{
    public class LemonMovement : MonoBehaviour
    {
        void LateUpdate()
        {
            if (CamLimitCoordinate.Instance.IsOutOfLimit(transform.position))
            {
                Destroy(this.gameObject);
            }
        }
    }
}
