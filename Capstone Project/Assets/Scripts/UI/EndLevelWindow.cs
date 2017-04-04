using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class EndLevelWindow : GenericWindow {

    public static event GenericWindowEvent OnContinue;
    public static event GenericWindowEvent OnBackToMain;

    [SerializeField]
    private GameObject _continueButton;
    [SerializeField]
    private GameObject _backToMainButton;

    private SOSaveFile _SOSaveHandler;
    private Text[] _stats;

    private int _currentLevel = 0;

    protected override void OnEnable() {
        base.OnEnable();

        _currentLevel = Application.loadedLevel;

        _SOSaveHandler = Resources.Load("ScriptableObjects/PlayerSaveFile", typeof(SOSaveFile)) as SOSaveFile;

        _stats = GameObject.Find("STATS NUMBERS").GetComponentsInChildren<Text>();

        _stats[0].text = _SOSaveHandler.CurrentDeathCount.ToString();
        _stats[1].text = (_SOSaveHandler.InProgressJouleShots + _SOSaveHandler.InProgressPersuaderShots).ToString();

        //switch (_currentLevel) {
        //    case 1:
        //        _stats[2].text = FormatTime(_SOSaveHandler.CurrentLevel1Time);
        //        _stats[3].text = FormatTime(_SOSaveHandler.BestLevel1Time);
        //        break;
        //    case 2:
        //        _stats[2].text = FormatTime(_SOSaveHandler.CurrentLevel1Time);
        //        _stats[3].text = FormatTime(_SOSaveHandler.BestLevel1Time);
        //        break;
        //    case 3:
        //        _stats[2].text = FormatTime(_SOSaveHandler.CurrentLevel1Time);
        //        _stats[3].text = FormatTime(_SOSaveHandler.BestLevel1Time);
        //        break;
        //}

        if (_currentLevel == 3) {
            _continueButton.SetActive(false);
            firstSelected = _backToMainButton;
        }
        else {
            _continueButton.SetActive(true);
            firstSelected = _continueButton;
        }
    }

    private void Update() {
        switch (_currentLevel) {
            case 1:
                _stats[2].text = FormatTime(_SOSaveHandler.CurrentLevel1Time);
                _stats[3].text = FormatTime(_SOSaveHandler.BestLevel1Time);
                break;
            case 2:
                _stats[2].text = FormatTime(_SOSaveHandler.CurrentLevel2Time);
                _stats[3].text = FormatTime(_SOSaveHandler.BestLevel2Time);
                break;
            case 3:
                _stats[2].text = FormatTime(_SOSaveHandler.CurrentLevel3Time);
                _stats[3].text = FormatTime(_SOSaveHandler.BestLevel3Time);
                break;
        }
    }

    public void ContinueButton() {
        if (OnContinue != null) { OnContinue(WindowIDs.EndOfLevelWindow, WindowIDs.None); }
    }

    public void BackToMainButton() {
        if (OnBackToMain != null) { OnBackToMain(WindowIDs.EndOfLevelWindow, WindowIDs.StartWindow); }
    }

    string FormatTime(float value) {
        TimeSpan timeSpan = TimeSpan.FromSeconds(value);
        return string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
    }
}
