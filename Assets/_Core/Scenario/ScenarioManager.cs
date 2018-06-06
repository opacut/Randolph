using System;
using System.Collections;
using UnityEngine;

namespace Assets.Core.Scenario {
    public abstract class ScenarioManager : MonoBehaviour {
        [SerializeField] private GameObject[] scenarioEvents;

        private int index = 0;
        private IEnumerator scenarioEnumerator;

        protected abstract IEnumerable Scenario();

        private void Awake() {
            scenarioEnumerator = Scenario().GetEnumerator();
            scenarioEvents[index++].GetComponent<IScenarioEventSource>().OnScenarioEvent += Iterate;
        }

        private void Iterate() {
            scenarioEvents[index - 1].GetComponent<IScenarioEventSource>().OnScenarioEvent -= Iterate;
            if (scenarioEnumerator.MoveNext()) {
                scenarioEvents[index++].GetComponent<IScenarioEventSource>().OnScenarioEvent += Iterate;
            }
        }
    }
}
