using UnityEngine;
using System.Collections;

public class LevelSelectWindow : GenericWindow {

    public static event GenericWindowEvent OnBackButton;

    private SOSaveFile _SOSaveHandler;

    public void BackButton() {
        if (OnBackButton != null) { OnBackButton(WindowIDs.LevelSelectWindow, WindowIDs.StartWindow); }
    }
}
