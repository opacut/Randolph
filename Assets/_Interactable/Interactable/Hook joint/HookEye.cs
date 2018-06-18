using UnityEngine;
using Randolph.Characters;
using Randolph.Core;

namespace Randolph.Interactable {
    public class HookEye : Interactable {

        public void Activate() => Constants.Randolph.GrappleTo(transform.position);
    }
}
