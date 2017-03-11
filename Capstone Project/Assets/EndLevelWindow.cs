using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndLevelWindow : GenericWindow {

    public static event GenericWindowEvent OnContinue;
    public static event GenericWindowEvent OnBackToMain;

    private SOSaveFile _SOSaveHandler;
    private Text[] _stats;

    protected override void OnEnable() {
        base.OnEnable();

        _SOSaveHandler = Resources.Load("ScriptableObjects/PlayerSaveFile", typeof(SOSaveFile)) as SOSaveFile;

        _stats = GameObject.Find("STATS NUMBERS").GetComponentsInChildren<Text>();

        _stats[0].text = _SOSaveHandler.CurrentDeathCount.ToString();
        _stats[1].text = (_SOSaveHandler.InProgressJouleShots + _SOSaveHandler.InProgressPersuaderShots).ToString();
        //_stats[2].text = _SOSaveHandler.PersuaderShots.ToString();
        //_stats[3].text = _SOSaveHandler.JouleShots.ToString();
    }

    public void ContinueButton() {
        if (OnContinue != null) { OnContinue(WindowIDs.EndOfLevelWindow, WindowIDs.None); }
    }

    public void BackToMainButton() {
        if (OnBackToMain != null) { OnBackToMain(WindowIDs.EndOfLevelWindow, WindowIDs.StartWindow); }
    }
}
