using UnityEngine;
using System.Collections;

public class WindowManager : Singleton<WindowManager> {

    [SerializeField]
    private WindowIDs _defaultWindowID;
    [SerializeField]
    private WindowIDs _currentWindowID;

    [SerializeField]
    private GenericWindow[] windows;

    private GameManager _GM;

    protected override void Awake() {
        _GM = GameManager.Instance;
        ToggleWindows(WindowIDs.None, WindowIDs.StartWindow);
    }

    private void OnEnable() {
        // Start Window
        StartWindow.OnContinue += ToggleWindows;
        StartWindow.OnNewGame += ToggleWindows;
        StartWindow.OnLevelSelect += ToggleWindows;
        StartWindow.OnStats += ToggleWindows;
        StartWindow.OnCredits += ToggleWindows;

        // Level Select


        // Stats
        StatsWindow.OnBack += ToggleWindows;

        // Credits
    }

    private void OnDisable() {
        // Start Window
        StartWindow.OnContinue -= ToggleWindows;
        StartWindow.OnNewGame -= ToggleWindows;
        StartWindow.OnLevelSelect -= ToggleWindows;
        StartWindow.OnStats -= ToggleWindows;
        StartWindow.OnCredits -= ToggleWindows;

        // Level Select


        // Stats
        StatsWindow.OnBack -= ToggleWindows;

        // Credits
    }

    private void ToggleWindows(WindowIDs close, WindowIDs open) {

        if (close != WindowIDs.None) { windows[(int)close].Close(); }
        _currentWindowID = open;

        if (_currentWindowID != WindowIDs.None) { windows[(int)_currentWindowID].Open(); }
    }
}
