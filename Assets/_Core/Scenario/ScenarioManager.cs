using System.Collections;
using Randolph.Levels;
using UnityEngine;

namespace Assets.Core.Scenario {
    public abstract class ScenarioManager : MonoBehaviour, IRestartable {
        private IEnumerator scenarioEnumerator;

        protected abstract IEnumerable Scenario();

        private void Start() => Restart();

        protected void Iterate() => scenarioEnumerator.MoveNext();

        #region IRestartable
        public virtual void SaveState() { }

        public virtual void Restart() {
            scenarioEnumerator = Scenario().GetEnumerator();
            Iterate();
        }
        #endregion
    }
}
