using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WindowManager : Singleton<WindowManager> {

    [SerializeField]
    private Image _title;
    [SerializeField]
    private WindowIDs _defaultWindowID;

    public WindowIDs currentWindowID;

    [SerializeField]
    private GenericWindow[] windows;

    private GameManager _GM;
    private Camera _camera;
    private int controllerType = -1;

    protected override void Awake() {
        _GM = GameManager.Instance.GetComponent<GameManager>(); ;
        _camera = Camera.main;
        currentWindowID = WindowIDs.None;
        DontDestroyOnLoad(gameObject);

        // Get a list of all availabe gamepads
        string[] inputs = Input.GetJoystickNames();

        // Determine if the player is using a Dualshock or Xbox controller
        // Stop looking after the first one is found.
        foreach (string input in inputs) {
            if (input != "") {
                controllerType = input == "Wireless Controller" ? 0 : 1;
                break;
            }
        }

        StandaloneInputModule eventSystem = EventSystemSingleton.Instance.GetComponent<StandaloneInputModule>();
        if (controllerType == 0) {
            eventSystem.horizontalAxis = "DS_DPAD_X";
            eventSystem.verticalAxis = "DS_DPAD_Y";
            eventSystem.submitButton = "DS_X";
        }
        else {
            eventSystem.horizontalAxis = "XBOX_DPAD_X";
            eventSystem.verticalAxis = "XBOX_DPAD_Y";
            eventSystem.submitButton = "XBOX_A";
        }

        DontDestroyOnLoad(eventSystem);
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

        // In Game
        EndOfLevel.OnLevelComplete += ToggleWindows;
        EndLevelWindow.OnContinue += ToggleWindows;
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

        // In Game
        EndOfLevel.OnLevelComplete -= ToggleWindows;
        EndLevelWindow.OnContinue -= ToggleWindows;
    }

    private void Update() {
        
    }

    public void ToggleWindows(WindowIDs close, WindowIDs open) {

        // Stop all coroutines so there is no ambiguity as to which window should be shown
        StopAllCoroutines();

        // Transitions Checks
        if (close == WindowIDs.StartWindow && open == WindowIDs.StatsWindow) { StartCoroutine(StartToStatsTransition()); }
        else if (close == WindowIDs.StatsWindow && open == WindowIDs.StartWindow) { StartCoroutine(StatsToStartTransition()); }

        // CHECK TO SEE WHEN THE TITLE SHOULD BE DISPLAYED OR NOT
        if (open == WindowIDs.None || open == WindowIDs.EndOfLevelWindow) { _title.enabled = false; }
        else { if (!_title.enabled) _title.enabled = true; }


        // ACTUAL OPENING CLOSING WINDOWS
        if (close != WindowIDs.None) { windows[(int)close].Close(); }
        currentWindowID = open;

        if (currentWindowID != WindowIDs.None) { windows[(int)currentWindowID].Open(); }
    }

    private IEnumerator StartToStatsTransition() {
        float _timer = Time.time;
        if (_camera == null) { _camera = Camera.main; }

        while(_camera.transform.position.y > -17.0f) {
            _camera.transform.position = new Vector3(_camera.transform.position.x, Mathf.SmoothStep(_camera.transform.position.y, -17.0f, (Time.time - _timer) / 2.5f), _camera.transform.position.z);
            yield return 0;
        }
    }

    private IEnumerator StatsToStartTransition() {
        float _timer = Time.time;
        if (_camera == null) { _camera = Camera.main; }

        while (_camera.transform.position.y < 0.0f) {
            _camera.transform.position = new Vector3(_camera.transform.position.x, Mathf.SmoothStep(_camera.transform.position.y, 0.0f, (Time.time - _timer) / 2.5f), _camera.transform.position.z);
            yield return 0;
        }
    }
}
