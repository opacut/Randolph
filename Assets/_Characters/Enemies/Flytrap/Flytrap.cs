using UnityEngine;
using Randolph.Interactable;

namespace Randolph.Characters
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Flytrap : MonoBehaviour, IEnemy
    {
        public bool Active { get; private set; } = true;

        Sprite alive;
        [Space(10), SerializeField] Sprite closed;
        [SerializeField] Sprite crushed;

        SpriteRenderer spriteRenderer;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            alive = spriteRenderer.sprite;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (Active)
            {
                if (other.tag == "Player")
                {
                    Deactivate();
                    other.gameObject.GetComponent<PlayerController>().Kill(1);
                }

                var glider = other.GetComponent<Glider>();
                if (glider)
                {
                    //! Crows flying into the flytrap
                    Deactivate();
                    glider.Kill();
                }
            }
        }

        public void Deactivate()
        {
            spriteRenderer.sprite = closed;
            Active = false;
        }

        public void Kill()
        {
            spriteRenderer.sprite = crushed;
            Active = false;
        }

        public void Restart()
        {
            spriteRenderer.sprite = alive;
            Active = true;
        }
    }
}
