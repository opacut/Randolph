using System.Collections;
using System.Collections.Generic;
using Randolph.Characters;
using Randolph.Core;
using Randolph.Interactable;
using Randolph.Levels;
using UnityEngine;

namespace Randolph.Characters {
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Animator))]
    public class Runner : MonoBehaviour, IEnemy {
        public float speed;
        public Sprite dead;

        bool alive = true;
        int goingRight;

        Animator animator;
        Vector3 initialPosition;
        Quaternion initialRotation;

        SpriteRenderer spriteRenderer;

        void Awake() {
            initialPosition = gameObject.transform.position;
            initialRotation = gameObject.transform.rotation;
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
        }

        void Start() {
            goingRight = 1;
        }

        void Update() {
            if (!alive)
                return;
            transform.Translate(goingRight * Vector2.right * speed * Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (!alive || other.isTrigger || other.GetComponent<Squasher>() != null || other.tag == Constants.Tag.Player)
                return;
            Turn();
        }

        private void OnCollisionEnter2D(Collision2D collision) {
            Collider2D collider = collision.collider;
            Vector3 contactPoint = collision.contacts[0].point;
            Vector3 center = collider.bounds.center;

            if (alive) {
                if (collider.tag == "Player") {
                    collider.gameObject.GetComponent<PlayerController>().Kill(1);
                } else if (collider.gameObject.GetComponent<Squasher>() != null) {
                    Kill();
                }
            }
        }

        void Turn() {
            Debug.Log("Turning");
            goingRight *= -1;

            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }

        public void Kill() {
            alive = false;
            animator.SetBool("Alive", false);
            spriteRenderer.sprite = dead;
        }

        public void Restart() {
            gameObject.transform.position = initialPosition;
            gameObject.transform.rotation = initialRotation;
            alive = true;
            animator.SetBool("Alive", true);
        }
    }
}