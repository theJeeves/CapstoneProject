using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartWindow : GenericWindow {

    public delegate void StartWindowEvent();
    public static event StartWindowEvent OnNewGame;
    public static event StartWindowEvent OnContinue;

    [SerializeField]
    private Button _continueButton;

    private GameManager _GM;

    protected override void Awake() {

    }

    private void OnEnable() {
        _GM = GameObject.Find("_GameManager").GetComponent<GameManager>();
    }

    private void Start() {
        Open();
    }

    public override void Open() {

        bool canContinue = _GM.SOSaveHandler.SOCheckpointHandler.checkPointReached;
        _continueButton.gameObject.SetActive(canContinue);

        if (_continueButton.gameObject.activeSelf) {
            firstSelected = _continueButton.gameObject;
        }

        base.Open();
    }

    public void Continue() {
        if (OnContinue != null) {
            OnContinue();
        }

        SceneManager.LoadScene(_GM.SOSaveHandler.currentLevel);
    }

    public void NewGame() {
        if (OnNewGame != null) {
            OnNewGame();
        }
        SceneManager.LoadScene(1);
    }
}
