using Randolph.Core;
using Randolph.UI;
using UnityEngine;

namespace Randolph.Levels {
    public class ShowSavedData : MonoBehaviour {

        readonly string levelKey = LevelManager.LevelKey;
        readonly string checkpointKey = CheckpointContainer.CheckpointKey;
        readonly string muteKey = MuteSwitch.MuteKey;

        // readonly string languageKey = "XXX";

        int _offset = 10;

        int CurrentOffset {
            // autoincrementing
            get {
                int result = _offset;
                _offset += 30;
                return result;
            }
            set { _offset = value; }
        }

        void OnGUI() {
            DisplayPlayerPrefs();
        }

        void DisplayPlayerPrefs() {
            Vector2 rectSize = new Vector2(200, 30);
            CurrentOffset = 10;

            int levelIndex = PlayerPrefs.HasKey(levelKey) ? PlayerPrefs.GetInt(levelKey) : 0;
            GUI.Label(new Rect(new Vector2(10, CurrentOffset), rectSize),
                    $"<color=red>{nameof(levelKey)}</color>: <color=blue>{levelIndex}</color>");

            int checkpointIndex = PlayerPrefs.HasKey(checkpointKey) ? PlayerPrefs.GetInt(checkpointKey) : 0;
            GUI.Label(new Rect(new Vector2(10, CurrentOffset), rectSize),
                    $"<color=red>{nameof(checkpointKey)}</color>: <color=blue>{checkpointIndex}</color>");

            GUI.Label(new Rect(new Vector2(10, CurrentOffset), rectSize),
                    $"<color=red>{nameof(LanguageSwitch.LanguageKey)}</color>: <color=blue>{LanguageSwitch.Language}</color>");

            bool mute = (PlayerPrefs.HasKey(muteKey) && PlayerPrefs.GetInt(muteKey) == 1);
            GUI.Label(new Rect(new Vector2(10, CurrentOffset), rectSize),
                    $"<color=red>{nameof(muteKey)}</color>: <color=blue>{mute}</color>");

            GUI.Label(new Rect(new Vector2(10, CurrentOffset), rectSize),
                    $"<color=red>{nameof(AudioPlayer.VolumeKey)}</color>: <color=blue>{AudioPlayer.GlobalVolume}</color>");

            if (GUI.Button(new Rect(new Vector2(10, CurrentOffset), rectSize), "Delete All Keys")) {
                PlayerPrefs.DeleteAll();
            }
        }

    }
}
