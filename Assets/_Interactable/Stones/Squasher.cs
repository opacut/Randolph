using UnityEngine;

using Randolph.Characters;
using Randolph.Levels;

namespace Randolph.Interactable
{
    public class Squasher : MonoBehaviour, IRestartable
    {
        Vector3 initialPosition;
        Quaternion initialRotation;

        void Awake()
        {
            initialPosition = gameObject.transform.position;
            initialRotation = gameObject.transform.rotation;
        }

        public void Restart()
        {
            gameObject.transform.position = initialPosition;
            gameObject.transform.rotation = initialRotation;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            // TODO: If any enemy
            if (other.tag == "Enemy")
            {
                other.gameObject.GetComponent<Flytrap>().Kill();
            }
        }
    }
}
