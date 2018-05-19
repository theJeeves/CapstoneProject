using UnityEngine;
using UnityEngine.UI;

public class StartWindow : GenericWindow {

    public static event GenericWindowEvent OnContinue;
    public static event GenericWindowEvent OnNewGame;
    public static event GenericWindowEvent OnLevelSelect;
    public static event GenericWindowEvent OnStats;
    public static event GenericWindowEvent OnCredits;
    //public static event GenericWindowEvent OnQuit;

    [SerializeField]
    private Button _continueButton;
    [SerializeField]
    private Button _start;
    [SerializeField]
    private Button _levelSelect;
    [SerializeField]
    private Button _stats;
    [SerializeField]
    private Button _credits;

    public Image _title;

    private GameObject _lastSelected;

    protected override void OnEnable() {
        base.OnEnable();
    }

    public override void Open() {

        if (_GM == null) { _GM = GameManager.Instance.GetComponent<GameManager>(); }

        _title.enabled = true;

        // IF A SAVE FILE IS FOUND, DISPLAY THE CONTINUE BUTTON
        bool canContinue = _GM.SOSaveHandler.CurrentLevel > 1 || _GM.SOSaveHandler.CheckpointID > 0 ? true : false;
        _continueButton.gameObject.SetActive(canContinue);

        if (_lastSelected != null) {
            firstSelected = _lastSelected;
        }
        else if (_continueButton.gameObject.activeSelf) {
            firstSelected = _continueButton.gameObject;
        }
        else {
            firstSelected = _start.gameObject;
        }

        base.Open();
    }

    public override void Close() {
        base.Close();
    }

    public void Continue() {
        _lastSelected = null;
        if (OnContinue != null) { OnContinue(WindowIDs.StartWindow, WindowIDs.None); }
    }

    public void NewGame() {
        _lastSelected = null;
        if (OnNewGame != null) { OnNewGame(WindowIDs.StartWindow, WindowIDs.None); }
    }

    public void LevelSelect() {
        _lastSelected = _levelSelect.gameObject;
        if (OnLevelSelect != null) { OnLevelSelect(WindowIDs.StartWindow, WindowIDs.LevelSelectWindow); }
    }

    public void Stats() {
        _lastSelected = _stats.gameObject;
        if (OnStats != null) { OnStats(WindowIDs.StartWindow, WindowIDs.StatsWindow); }
    }

    public void Credits() {
        _lastSelected = _credits.gameObject;
        if (OnCredits != null) { OnCredits(WindowIDs.StartWindow, WindowIDs.CreditsWindow); }
    }

    public void Quit() {
        Application.Quit();
    }
}
