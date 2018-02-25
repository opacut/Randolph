using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class Exit : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private List<Objective> objectives = new List<Objective>();


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Player")
            return;

        if (objectives.All(objective => objective.IsCompleted))
        {
            SceneManager.LoadScene("Finish");
        }
        else
        {
            anim.SetTrigger("ObjectiveNotFound");
        }
    }
}
