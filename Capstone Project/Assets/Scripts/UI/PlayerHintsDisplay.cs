using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerHintsDisplay : MonoBehaviour {

    private Text _hints;

    private void Awake() {
        _hints = GetComponent<Text>();
    }

    private void OnEnable() {
        InstructionText.DisplayHint += DisplayHint;
        //InstructionText.HideHint += HideHint;
    }

    private void OnDisable() {
        InstructionText.DisplayHint -= DisplayHint;
        //InstructionText.HideHint -= HideHint;
    }

    private void DisplayHint(string hint) {

        hint = hint.Replace(" NL ", "\n");
        _hints.text = hint;
    }
}
