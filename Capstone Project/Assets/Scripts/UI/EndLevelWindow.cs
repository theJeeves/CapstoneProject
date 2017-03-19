using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndLevelWindow : GenericWindow {

    public static event GenericWindowEvent OnContinue;
    public static event GenericWindowEvent OnBackToMain;

    [SerializeField]
    private GameObject _continueButton;
    [SerializeField]
    private GameObject _backToMainButton;

    private SOSaveFile _SOSaveHandler;
    private Text[] _stats;

    protected override void OnEnable() {
        base.OnEnable();

        _SOSaveHandler = Resources.Load("ScriptableObjects/PlayerSaveFile", typeof(SOSaveFile)) as SOSaveFile;

        _stats = GameObject.Find("STATS NUMBERS").GetComponentsInChildren<Text>();

        _stats[0].text = _SOSaveHandler.CurrentDeathCount.ToString();
        _stats[1].text = (_SOSaveHandler.InProgressJouleShots + _SOSaveHandler.InProgressPersuaderShots).ToString();

        if (Application.loadedLevel == 3) {
            _continueButton.SetActive(false);
            firstSelected = _backToMainButton;
        }
        else {
            _continueButton.SetActive(true);
            firstSelected = _continueButton;
        }
    }

    public void ContinueButton() {
        if (OnContinue != null) { OnContinue(WindowIDs.EndOfLevelWindow, WindowIDs.None); }
    }

    public void BackToMainButton() {
        if (OnBackToMain != null) { OnBackToMain(WindowIDs.EndOfLevelWindow, WindowIDs.StartWindow); }
    }
}
