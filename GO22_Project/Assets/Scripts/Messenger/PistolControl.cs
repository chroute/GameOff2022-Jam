using UnityEngine;

namespace GO22
{
    public class PistolControl : MonoBehaviour
    {
        [SerializeField]
        private int xIncrement = 3;
        private Vector2 input;
        private ParticleSystem ps;
        private MessengerGameControl gameControl;
        private int pos;
        private bool fire;

        private void Start()
        {
            ps = GetComponentInChildren<ParticleSystem>();
            gameControl = GetComponent<MessengerGameControl>();
        }

        void Update()
        {
            fire = false;
            int move = 0;
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                move = -1;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                move = 1;
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                fire = true;
            }
            pos += move;
            pos = Mathf.Clamp(pos, -1, 1);
            Vector2 current = transform.position;
            current.x = pos * xIncrement;
            transform.position = current;
        }

        private void LateUpdate() {
            if (fire) {
                Fire();
            }
        }

        private void Fire()
        {
            ps.Play();
            gameControl.FireAtPosition(pos);
            AudioManager.Instance?.Play("GunShoot");

        }
    }
}
