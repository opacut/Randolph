using System;
using System.Threading.Tasks;
using Randolph.Core;
using UnityEngine;

namespace Randolph.Interactable {
    public class TiedRope : Interactable {

        public override void OnInteract() {
            Debug.Log("Rope clicked.");
        }
    }
}