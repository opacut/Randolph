﻿using UnityEngine;

namespace Randolph.Interactable {
    public class GrassTuft : Interactable, ISlashable {
        [SerializeField] private Sprite cutGrass;
        [SerializeField] private DryGrass grassPrefab;

        private bool hasGrass = true;
        private Sprite original;

        public void Slash() {
            if (!hasGrass) {
                return;
            }
            hasGrass = false;
            spriteRenderer.sprite = cutGrass;
            var newGrass = Instantiate(grassPrefab, transform.parent);
            newGrass.Pick();
        }

        protected override void Awake() {
            base.Awake();
            original = spriteRenderer.sprite;
        }

        public override void Restart() {
            base.Restart();
            hasGrass = true;
            spriteRenderer.sprite = original;
        }
    }
}
