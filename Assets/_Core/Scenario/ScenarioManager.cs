using System;
using System.Collections;
using UnityEngine;

namespace Assets.Core.Scenario {
    public abstract class ScenarioManager : MonoBehaviour {
        protected Action scenarioEvent;
        private IEnumerator scenarioEnumerator;

        protected abstract IEnumerable Scenario();

        private void Awake() {
            scenarioEnumerator = Scenario().GetEnumerator();
            Iterate();
        }

        protected void Iterate() => scenarioEnumerator.MoveNext();
    }
}
