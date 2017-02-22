using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerHintsDisplay : MonoBehaviour {

    private Text _hints;

    private void Awake() {
        _hints = GetComponent<Text>();
    }

    private void OnEnable() {
        Hint_Controls.DisplayHint += DisplayHint;
        Hint_Controls.HideHint += HideHint;
    }

    private void OnDisable() {
        Hint_Controls.DisplayHint -= DisplayHint;
        Hint_Controls.HideHint -= HideHint;
    }

    private void DisplayHint(string hint) {

        hint = hint.Replace(" NL ", "\n");
        _hints.text = hint;
    }

    private void HideHint() {
        _hints.text = "";
    }
}
