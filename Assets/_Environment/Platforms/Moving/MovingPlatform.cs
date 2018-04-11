﻿using UnityEngine;
using Randolph.Characters;

namespace Randolph.Environment {
    [RequireComponent(typeof(Glider))]
    public class MovingPlatform : MonoBehaviour {

        [SerializeField] GameObject chainLink;
        [SerializeField] Sprite chainCorner;
        [SerializeField] bool visibleChain = false;
        Transform chainHolder;

        Transform attachedPlayer;
        Animator animator;
        Vector2 lastPosition;


        // [ExecuteInEditMode]
        void Awake() {
            animator = GetComponent<Animator>();
            lastPosition = transform.position;

            if (visibleChain) ConstructChain();
        }

        void ConstructChain() {
            if (transform.childCount == 0) Instantiate(new GameObject("Chain"), transform);
            chainHolder = transform.GetChild(0);
            if (chainHolder.childCount > 0) {
                // Remove old chain
                for (int i = 0; i < transform.childCount; i++) {
                    Destroy(transform.GetChild(i));
                }
            }
        }

        void FixedUpdate() {
            if (attachedPlayer != null) {
                Vector2 positionChange = GetPositionChange();
                if (positionChange != Vector2.zero) {
                    // TODO: Not if there's input
                    attachedPlayer.position += new Vector3(positionChange.x, positionChange.y, 0f);
                }
            }

            animator.SetBool("Move", lastPosition != (Vector2) transform.position); // Move animation when moving
            lastPosition = transform.position;
        }

        void OnCollisionEnter2D(Collision2D collision) {
            if (collision.gameObject.tag == "Player") {
                attachedPlayer = collision.transform;
            }
        }

        void OnCollisionExit2D(Collision2D collision) {
            if (collision.gameObject.tag == "Player") {
                attachedPlayer = null;
            }
        }

        Vector2 GetPositionChange() {
            if (Mathf.Approximately(Vector2.Distance(transform.position, lastPosition), 0f)) {
                return Vector2.zero;
            } else {
                return transform.position - new Vector3(lastPosition.x, lastPosition.y, 0f);
            }
        }

    }
}
