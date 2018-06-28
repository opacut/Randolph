using System.Collections;
using Randolph.Levels;

namespace Assets.Core.Scenario {
    public abstract class ScenarioManager : RestartableBase {
        private IEnumerator scenarioEnumerator;

        protected abstract IEnumerable Scenario();

        protected override void Start() {
            base.Start();
            Restart();
        }

        protected void Iterate() => scenarioEnumerator.MoveNext();

        #region IRestartable
        public override void Restart() {
            base.Restart();
            scenarioEnumerator = Scenario().GetEnumerator();
            Iterate();
        }
        #endregion
    }
}
