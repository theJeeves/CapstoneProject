using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartWindow : GenericWindow {

    [SerializeField]
    private Button _continueButton;

    protected override void Awake() {

    }

    private void Start() {
        Open();
    }

    public override void Open() {

        bool canContinue = false;
        _continueButton.gameObject.SetActive(canContinue);

        if (_continueButton.gameObject.activeSelf) {
            firstSelected = _continueButton.gameObject;
        }

        base.Open();
    }

    public void NewGame() {
        SceneManager.LoadScene(1);
    }
}
