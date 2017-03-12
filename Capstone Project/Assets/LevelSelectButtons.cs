using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelSelectButtons : GenericWindow {

    public static event GenericWindowEvent ToggleWindows;

    public delegate void LevelSelectEvent(int level);
    public static event LevelSelectEvent SelectLevel;

    [SerializeField]
    private Button[] _levelButtons;

    private Selectable _backButton;
    private bool _buttonSet = false;

    private void OnDisable() {
        _buttonSet = false;
        _backButton = null;
    }

    private void Update() {
        if (_backButton == null && GameObject.FindGameObjectWithTag("Back_LevelSelect") != null) {
            _backButton = GameObject.FindGameObjectWithTag("Back_LevelSelect").GetComponent<Selectable>();
        }

        if (!_buttonSet && _backButton != null) {
            for (int i = 0; i < _levelButtons.Length; ++i) {

                Navigation navigation = _levelButtons[i].navigation;
                navigation.selectOnLeft = _backButton;
                navigation.selectOnRight = _backButton;

                _levelButtons[i].navigation = navigation;
            }
            _buttonSet = true;
        }
    }


    public void Level_1() {
        if (SelectLevel != null) { SelectLevel(1); }
        if (ToggleWindows != null) { ToggleWindows(WindowIDs.LevelSelectWindow, WindowIDs.None); }
    }

    public void Level_2() {
        if (SelectLevel != null) { SelectLevel(2); }
        if (ToggleWindows != null) { ToggleWindows(WindowIDs.LevelSelectWindow, WindowIDs.None); }
    }

    public void Level_3() {
        if (SelectLevel != null) { SelectLevel(3); }
        if (ToggleWindows != null) { ToggleWindows(WindowIDs.LevelSelectWindow, WindowIDs.None); }
    }
}
