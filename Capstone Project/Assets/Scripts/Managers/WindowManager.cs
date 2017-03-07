﻿using UnityEngine;
using System.Collections;

public class WindowManager : Singleton<WindowManager> {

    [SerializeField]
    private WindowIDs _defaultWindowID;
    [SerializeField]
    private WindowIDs _currentWindowID;

    [SerializeField]
    private GenericWindow[] windows;

    private GameManager _GM;
    private Camera _camera;

    protected override void Awake() {
        _GM = GameManager.Instance;
        _camera = Camera.main;
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

    private void Update() {
        
    }

    private void ToggleWindows(WindowIDs close, WindowIDs open) {

        // Stop all coroutines so there is no ambiguity as to which window should be shown
        StopAllCoroutines();

        // Transitions Checks
        if (close == WindowIDs.StartWindow && open == WindowIDs.StatsWindow) { StartCoroutine(StartToStatsTransition()); }
        else if (close == WindowIDs.StatsWindow && open == WindowIDs.StartWindow) { StartCoroutine(StatsToStartTransition()); }

        if (close != WindowIDs.None) { windows[(int)close].Close(); }
        _currentWindowID = open;

        if (_currentWindowID != WindowIDs.None) { windows[(int)_currentWindowID].Open(); }
    }

    private IEnumerator StartToStatsTransition() {
        float _timer = Time.time;
        while(_camera.transform.position.y > -17.0f) {
            _camera.transform.position = new Vector3(_camera.transform.position.x, Mathf.SmoothStep(_camera.transform.position.y, -17.0f, (Time.time - _timer) / 2.5f), _camera.transform.position.z);
            yield return 0;
        }
    }

    private IEnumerator StatsToStartTransition() {
        float _timer = Time.time;
        while (_camera.transform.position.y < 0.0f) {
            _camera.transform.position = new Vector3(_camera.transform.position.x, Mathf.SmoothStep(_camera.transform.position.y, 0.0f, (Time.time - _timer) / 2.5f), _camera.transform.position.z);
            yield return 0;
        }
    }
}
