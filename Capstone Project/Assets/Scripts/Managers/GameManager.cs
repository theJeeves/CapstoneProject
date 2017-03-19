using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameManager : Singleton<GameManager> {

    public SOSaveFile SOSaveHandler;
    public SOEffects SOEffectHandler;

    private InputManager _IM;
    private WindowManager _WM;
    StandaloneInputModule eventSystem;
    private GameObject _player;

    private bool _inGame = false;
    private bool _paused = false;
    private float _defaultTimeScale = 1.0f;

    protected override void Awake() {
        // CALL BASE AWAKE TO ENSURE THERE ARE NO OTHER INSTANCES OF THE GAME MANAGER IN THE SCENE
        base.Awake();

        // LOAD THE SCRIPTABLEOBJECTS TO BE USED THROUGHOUT THE GAME.
        SOSaveHandler = Resources.Load("ScriptableObjects/PlayerSaveFile", typeof(SOSaveFile)) as SOSaveFile;
        SOEffectHandler = Resources.Load("ScriptableObjects/SOEffectHandler", typeof(SOEffects)) as SOEffects;

        _IM = InputManager.Instance.GetComponent<InputManager>();
        _WM = WindowManager.Instance.GetComponent<WindowManager>();
        eventSystem = EventSystemSingleton.Instance.GetComponent<StandaloneInputModule>();

        SOEffectHandler.LoadEffects();
    }

    private void OnEnable() {
        // Start Window Events
        StartWindow.OnContinue += OnContinue;
        StartWindow.OnNewGame += OnNewGame;

        // LEVEL SELECT
        LevelSelectButtons.SelectLevel += OnSelectLevel;

        // Player Death Event
        PlayerHealth.OnPlayerDeath += OnPlayerDeath;

        // Gun Events
        Shotgun.ShotFired += OnShotgunFired;
        MachineGun.ShotFired += OnMachineGunFired;

        // Enemy Death Events
        EnemyBasicBehaviors.OnDeath += OnEnemyDeath;

        // LEVEL COMPLETED
        EndOfLevel.OnLevelComplete += OnLevelComplete;
        EndLevelWindow.OnContinue += OnLoadNextLevel;
        EndLevelWindow.OnBackToMain += OnBackToMain;

        // PAUSE WINDOW
        PauseWindow.OnContinueButton += OnPauseContinue;
        PauseWindow.OnResartLevelButton += OnPauseRestartLevel;
        PauseWindow.OnBackToMainButton += OnPauseBackToMain;
    }

    private void OnDisable() {
        // Start Window Events
        StartWindow.OnContinue -= OnContinue;
        StartWindow.OnNewGame -= OnNewGame;

        // LEVEL SELECT
        LevelSelectButtons.SelectLevel += OnSelectLevel;

        // Player Death Event
        PlayerHealth.OnPlayerDeath -= OnPlayerDeath;

        // Gun Events
        Shotgun.ShotFired -= OnShotgunFired;
        MachineGun.ShotFired -= OnMachineGunFired;

        // Enemy Death Events
        EnemyBasicBehaviors.OnDeath -= OnEnemyDeath;

        // LEVEL COMPLETED Events
        EndOfLevel.OnLevelComplete -= OnLevelComplete;
        EndLevelWindow.OnContinue -= OnLoadNextLevel;
        EndLevelWindow.OnBackToMain -= OnBackToMain;

        // PAUSE WINDOW
        PauseWindow.OnContinueButton -= OnPauseContinue;
        PauseWindow.OnResartLevelButton -= OnPauseRestartLevel;
        PauseWindow.OnBackToMainButton -= OnPauseBackToMain;
    }

    private void Update() {
        if (_inGame && ( _IM.controllerType == 0 ? Input.GetButtonDown("DS_OPTIONS") : Input.GetButtonDown("XBOX_START")) ) {

            if (!_paused) {
                _paused = true;
                Time.timeScale = 0.0f;
                _WM.ToggleWindows(WindowIDs.None, WindowIDs.PauseWindow);
            }
            else if (_paused) {
                _paused = false;
                Time.timeScale = _defaultTimeScale;
                _WM.ToggleWindows(WindowIDs.PauseWindow, WindowIDs.None);
            }
        }
    }

    //
    // Start Window Events
    //
    public void OnContinue(WindowIDs ignore1, WindowIDs ignore2) {
        SceneManager.LoadScene(SOSaveHandler.CurrentLevel);
        _inGame = true;
    }

    private void OnNewGame(WindowIDs ignore1, WindowIDs ignore2) {
        SOSaveHandler.NewGame();
        SceneManager.LoadScene(1);
        _inGame = true;
    }

    //
    // LEVEL SELECT
    //
    private void OnSelectLevel(int level) {
        SOSaveHandler.LoadLevel(level);
        SceneManager.LoadScene(level);
        Time.timeScale = _defaultTimeScale;
        _inGame = true;
        _paused = false;
    }

    //
    // Player Death Event
    //
    // Add to the counter for every time the player dies
    private void OnPlayerDeath() {
        SOSaveHandler.CurrentDeathCount += 1;
        SOSaveHandler.DeathCount += 1;
    }

    //
    /// Gun Events
    //
    private void OnShotgunFired() {
        SOSaveHandler.InProgressJouleShots += 1;
        SOSaveHandler.JouleShots += 1;
    }

    private void OnMachineGunFired() {
        SOSaveHandler.InProgressPersuaderShots += 1;
        SOSaveHandler.PersuaderShots += 1;
    }

    //
    // Enemy Death Events
    //
    // Add to the counter for every kill the player makes
    private void OnEnemyDeath(EnemyType type) {
        switch (type) {
            case EnemyType.AcidSwarmer:
                SOSaveHandler.AcidVectorsKilled += 1; break;
            case EnemyType.ExplodingSwamer:
                SOSaveHandler.ExplosiveVectorsKilled += 1; break;
            case EnemyType.Flying:
                SOSaveHandler.FlyingVectorsKilled += 1; break;
            case EnemyType.Sniper:
                SOSaveHandler.SnipersKilled += 1; break;
            case EnemyType.Charger:
                SOSaveHandler.ChargersKilled += 1; break;
        }
    }

    //
    // LEVEL COMPLETED Events
    //
    private void OnLevelComplete(WindowIDs ignore1, WindowIDs ignore) {
        _IM.StopInput();
        _inGame = false;
        Time.timeScale = 0.0f;
    }

    private void OnLoadNextLevel(WindowIDs ignore1, WindowIDs ignore2) {
        SOSaveHandler.NextLevel();
        SceneManager.LoadScene(SOSaveHandler.CurrentLevel);
        _inGame = true;
        Time.timeScale = _defaultTimeScale;
        _paused = false;
    }

    private void OnBackToMain(WindowIDs close, WindowIDs open) {

        _inGame = false;
        _paused = false;

        if (Application.loadedLevel < 3) {
            SOSaveHandler.NextLevel();
        }

        SceneManager.LoadScene(0);
        Time.timeScale = _defaultTimeScale;
    }


    //
    // PAUSE MENU EVENTS
    //
    private void OnPauseContinue(WindowIDs ignore1, WindowIDs ignore2) {
        _paused = false;
        Time.timeScale = _defaultTimeScale;
    }

    private void OnPauseRestartLevel(WindowIDs ignore1, WindowIDs ignore2) {
        SOSaveHandler.RestartLevel();
        SceneManager.LoadScene(SOSaveHandler.CurrentLevel);
        _paused = false;
        Time.timeScale = _defaultTimeScale;
    }

    private void OnPauseBackToMain(WindowIDs ignore1, WindowIDs ignore2) {
        _paused = false;
        SceneManager.LoadScene(0);
        Time.timeScale = _defaultTimeScale;
        _inGame = false;
    }

    // Things to do when a level is loaded
    private void OnLevelWasLoaded(int level) {

        if (level != 0) {
            _inGame = true;

            if (SOSaveHandler.CheckpointID == 0) {
                GameObject player = SpawnPlayer();

                if (level == 1) {
                    player.GetComponentInChildren<WeaponSelect>().MGAvailable = true;
                    player.GetComponentInChildren<WeaponSelect>().SGAvailable = false;
                    _IM.StopInput();
                }
            }
            else {
                SpawnPlayer();
                GameObject[] checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
                GameObject camera = GameObject.FindGameObjectWithTag("SmartCamera");

                foreach (GameObject ckpt in checkpoints) {

                    if (ckpt.GetComponent<CheckpointBehavior>().ID == SOSaveHandler.CheckpointID) {
                        CheckpointBehavior ckptBehavior = ckpt.GetComponent<CheckpointBehavior>();

                        ckptBehavior.Activate();
                        camera.transform.position = ckptBehavior.cameraPos;

                        if (ckptBehavior.smartXEnabled) {
                            camera.GetComponent<SmartCameraXPosition>().enabled = true;
                        }
                        else {
                            camera.GetComponent<SmartCameraXPosition>().enabled = false;
                        }
                        if (ckptBehavior.smartYEnabled) {
                            camera.GetComponent<SmartCameraYPosition>().enabled = true;
                        }
                        else {
                            camera.GetComponent<SmartCameraYPosition>().enabled = false;
                        }
                    }
                }
            }
        }
        else if (level == 0) {
            _inGame = false;
        }
    }

    private GameObject SpawnPlayer() {
        GameObject _player = Instantiate(Resources.Load("MainCharacter/MainCharacter", typeof(GameObject)) as GameObject, transform.position, Quaternion.identity) as GameObject;
        SOEffectHandler.PlayEffect(EffectEnums.PlayerRespawn, SOSaveHandler.CheckpointPosition);
        _player.transform.position = SOSaveHandler.CheckpointPosition;
        _IM.AssignPlayer(_player);
        if (SOSaveHandler.CurrentLevel > 1) { _player.GetComponentInChildren<WeaponSelect>().SGAvailable = true; }
        else if (SOSaveHandler.CurrentLevel == 1 && SOSaveHandler.CheckpointID >= 7 && SOSaveHandler.CheckpointID != 8) { _player.GetComponentInChildren<WeaponSelect>().SGAvailable = true; }
        else { _player.GetComponentInChildren<WeaponSelect>().SGAvailable = false; }
        return _player;
    }
}
