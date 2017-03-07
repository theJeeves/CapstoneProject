using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StartWindow : GenericWindow {

    public delegate void StartWindowEvent(WindowIDs close, WindowIDs open);
    public static event StartWindowEvent OnContinue;
    public static event StartWindowEvent OnNewGame;
    public static event StartWindowEvent OnLevelSelect;
    public static event StartWindowEvent OnStats;
    public static event StartWindowEvent OnCredits;
    public static event StartWindowEvent OnQuit;

    [SerializeField]
    private Button _continueButton;

    public Image _title;

    protected override void OnEnable() {
        base.OnEnable();
    }

    public override void Open() {

        if (_GM == null) { _GM = GameManager.Instance; }

        // IF A SAVE FILE IS FOUND, DISPLAY THE CONTINUE BUTTON
        bool canContinue = _GM.SOSaveHandler.CurrentLevel > 1 || _GM.SOSaveHandler.CheckpointID > 0 ? true : false;
        _continueButton.gameObject.SetActive(canContinue);

        if (_continueButton.gameObject.activeSelf) {
            firstSelected = _continueButton.gameObject;
        }

        base.Open();
    }

    public override void Close() {
        base.Close();
    }

    public void Continue() {
        if (OnContinue != null) { OnContinue(WindowIDs.StartWindow, WindowIDs.None); }
    }

    public void NewGame() {
        if (OnNewGame != null) { OnNewGame(WindowIDs.StartWindow, WindowIDs.None); }
    }

    public void LevelSelect() {
        if (OnLevelSelect != null) { OnLevelSelect(WindowIDs.StartWindow, WindowIDs.LevelSelectWindow); }
    }

    public void Stats() {
        if (OnStats != null) { OnStats(WindowIDs.StartWindow, WindowIDs.StatsWindow); }
    }

    public void Credits() {
        if (OnCredits != null) { OnCredits(WindowIDs.StartWindow, WindowIDs.CreditsWindow); }
    }

    public void Quit() {
        Application.Quit();
    }
}
