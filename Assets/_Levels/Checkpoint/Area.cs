using System.Collections.Generic;

using Randolph.Environment;

using UnityEngine;

namespace Randolph.Levels {
    public class Area : MonoBehaviour {

        void Awake() {
            if (CheckNestedAreas()) {
                Debug.LogError($"<b>{gameObject.name}</b> is nested in another area. This can cause problems.", gameObject);
            }
        }

        bool CheckNestedAreas() {
            Area[] areasUpwards = GetComponentsInParent<Area>(true); // includes this area
            return areasUpwards.Length > 1;
        }

        public List<SpikeTrap> GetAreaSpikes() {
            var spikes = new List<SpikeTrap>();
            GetComponentsInChildren(true, spikes);
            return spikes;
        }

    }
}