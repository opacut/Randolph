using UnityEngine;
using Randolph.Environment;
using Assets._Interactable;

namespace Randolph.Interactable {
    public class Sabre : InventoryItem {

        public override bool IsSingleUse => false;

        public override bool IsApplicable(GameObject target) => target.GetComponent<ISlashable>() != null;

        public override void Apply(GameObject target) {
            base.Apply(target);
            target.GetComponent<ISlashable>().Slash();
            //Destroy(target);
        }
    }
}