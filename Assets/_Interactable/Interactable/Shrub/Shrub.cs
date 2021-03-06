﻿using UnityEngine;

namespace Randolph.Interactable {
    public class Shrub : Interactable, ISlashable {
        [SerializeField] public Twig twigPrefab;

        public void Slash() {
            var newTwig = Instantiate(twigPrefab, transform.parent);
            newTwig.Pick();
            gameObject.SetActive(false);
        }
    }
}
