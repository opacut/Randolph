using System.Collections;
using Randolph.UI;

namespace Assets.Core.Scenario {
    public class TestScenario : ScenarioManager {
        public SpeechBubble character;
        public string text1;
        public string text2;

        protected override IEnumerable Scenario() {
            character.fullText = text1;
            yield return null;
            character.fullText = text2;
        }
    }
}
