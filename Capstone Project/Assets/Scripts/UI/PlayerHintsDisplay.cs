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
        //int numOfHints = hint.Length;

        //if (numOfHints == 1) {
        //    _hints.text = hint[0].ToString();
        //}
        //else if (numOfHints > 1) {
        //    for (int i = 0; i < numOfHints; ++i) {
        //        if (i == numOfHints - 1) {
        //            _hints.text += hint[i].ToString();
        //        }
        //        else {
        //            _hints.text += hint[i].ToString() + " + ";
        //        }
        //    }
        //}
    }

    private void HideHint() {
        _hints.text = "";
    }
}
