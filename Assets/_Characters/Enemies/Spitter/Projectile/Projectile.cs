using UnityEngine;

namespace Randolph.Characters
{
    public class Projectile : MonoBehaviour
    {
        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Player")
            {
                other.gameObject.GetComponent<PlayerController>().Kill();
            }
        }
    }
}
