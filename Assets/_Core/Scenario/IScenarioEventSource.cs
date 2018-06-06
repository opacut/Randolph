using System;

namespace Assets.Core.Scenario {
    public interface IScenarioEventSource {
        event Action OnScenarioEvent;
    }
}