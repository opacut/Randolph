using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Randolph.Levels {
    [RequireComponent(typeof(Collider2D))]
    public class LevelExit : MonoBehaviour {
        [SerializeField] Animator animator;
        [SerializeField] List<Objective> objectives = new List<Objective>();

        void OnCollisionEnter2D(Collision2D collision) {
            if (collision.gameObject.tag != "Player") return;

            if (objectives.All(objective => objective.IsCompleted)) {
                LevelManager.LoadNextLevel();
            } else {
                animator.SetTrigger("ObjectiveNotFound");
            }
        }
    } 
}
