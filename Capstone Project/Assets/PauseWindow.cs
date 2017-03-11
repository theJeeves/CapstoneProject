using UnityEngine;
using System.Collections;

public class PauseWindow : GenericWindow {

    public static event GenericWindowEvent OnContinueButton;
    public static event GenericWindowEvent OnResartLevelButton;
    public static event GenericWindowEvent OnBackToMainButton;

    protected override void OnEnable() {
        base.OnEnable();
        firstSelected.GetComponent<ButtonBehavior>().HighlightButton();
    }

    public void ContinueButton() {
        if (OnContinueButton != null) {
            OnContinueButton(WindowIDs.PauseWindow, WindowIDs.None);
        }
    }

    public void ResartLevelButton() {
        if (OnResartLevelButton != null) { OnResartLevelButton(WindowIDs.PauseWindow, WindowIDs.None); }
    }

    public void BackToMainButton() {
        if (OnBackToMainButton != null) { OnBackToMainButton(WindowIDs.PauseWindow, WindowIDs.StartWindow); }
    }
}
