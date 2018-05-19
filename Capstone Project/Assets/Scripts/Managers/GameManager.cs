using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameManager : Singleton<GameManager> {

    #region Constant Fields
    private const string MAIN_CHARACTER_PATH = "MainCharacter/MainCharacter";
    private const string PLAYER_SAVE_FILE_PATH = "ScriptableObjects/PlayerSaveFile";
    private const string EFFECT_HANDLER_PATH = "ScriptableObjects/SOEffectHandler";
    private const string DUAL_SHOCK = "DS_OPTIONS";
    private const string XBOX = "XBOX_START";

    #endregion Constant Fields

    #region Public Fields
    public SOSaveFile SOSaveHandler;
    public SOEffects SOEffectHandler;

    #endregion Public Fields

    #region Private Fields
    private InputManager m_IM = null;
    private WindowManager m_WM = null;
    private StandaloneInputModule m_EventSystem = null;
    private GameObject m_Player = null;

    private bool m_InGame = false;
    private bool m_Paused = false;
    private float m_DefaultTimeScale = 1.0f;

    private int m_CurrentLevel = 0;
    private float m_PlayeTime = 0.0f;

    #endregion Private Fields

    #region Initializers
    protected override void Awake() {
        // CALL BASE AWAKE TO ENSURE THERE ARE NO OTHER INSTANCES OF THE GAME MANAGER IN THE SCENE
        base.Awake();

        // LOAD THE SCRIPTABLEOBJECTS TO BE USED THROUGHOUT THE GAME.
        SOSaveHandler = Resources.Load(PLAYER_SAVE_FILE_PATH, typeof(SOSaveFile)) as SOSaveFile;
        SOEffectHandler = Resources.Load(EFFECT_HANDLER_PATH, typeof(SOEffects)) as SOEffects;

        m_IM = InputManager.Instance.GetComponent<InputManager>();
        m_WM = WindowManager.Instance.GetComponent<WindowManager>();
        m_EventSystem = EventSystemSingleton.Instance.GetComponent<StandaloneInputModule>();

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
        EnemyBasicBehaviors.EnemyDeath += OnEnemyDeath;

        // stats events
        StatsWindow.OnResetStats += ResetStats;

        // LEVEL COMPLETED
        EndOfLevel.LevelComplete += OnLevelComplete;
        EndLevelWindow.OnContinue += OnLoadNextLevel;
        EndLevelWindow.OnBackToMain += OnBackToMain;

        // PAUSE WINDOW
        PauseWindow.OnContinueButton += OnPauseContinue;
        PauseWindow.OnResartLevelButton += OnPauseRestartLevel;
        PauseWindow.OnBackToMainButton += OnPauseBackToMain;

        // Scene Changes
        SceneManager.sceneLoaded += OnSceneLoaded;

        m_PlayeTime = 0.0f;
    }

    #endregion Initializers

    #region Finalizers
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
        EnemyBasicBehaviors.EnemyDeath -= OnEnemyDeath;

        // stats events
        StatsWindow.OnResetStats -= ResetStats;

        // LEVEL COMPLETED Events
        EndOfLevel.LevelComplete -= OnLevelComplete;
        EndLevelWindow.OnContinue -= OnLoadNextLevel;
        EndLevelWindow.OnBackToMain -= OnBackToMain;

        // PAUSE WINDOW
        PauseWindow.OnContinueButton -= OnPauseContinue;
        PauseWindow.OnResartLevelButton -= OnPauseRestartLevel;
        PauseWindow.OnBackToMainButton -= OnPauseBackToMain;

        // Scene Changes
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    #endregion Finalizers

    #region Public Methods
    /// <summary>
    /// Start Window Events
    /// </summary>
    /// <param name="ignore1"></param>
    /// <param name="ignore2"></param>
    public void OnContinue(WindowIDs ignore1, WindowIDs ignore2)
    {
        SceneManager.LoadScene(SOSaveHandler.CurrentLevel);
        m_InGame = true;
        m_Paused = false;

        switch (m_CurrentLevel)
        {
            case 1:
                m_PlayeTime = SOSaveHandler.CurrentLevel1Time; break;
            case 2:
                m_PlayeTime = SOSaveHandler.CurrentLevel2Time; break;
            case 3:
                m_PlayeTime = SOSaveHandler.CurrentLevel3Time; break;
        }
    }

    #endregion Public Methods

    #region Private Methods
    // This is where all the "Menu Controls" should go. Example, back button and pausing the game.
    private void Update() {
        if (m_InGame) {

            if (!m_Paused) {
                UpdateTime();
            }

            if (m_IM.controllerType == 0 ? Input.GetButtonDown(DUAL_SHOCK) : Input.GetButtonDown(XBOX) ) {

                if (!m_Paused) {
                    m_Paused = true;
                    Time.timeScale = 0.0f;
                    m_WM.ToggleWindows(WindowIDs.None, WindowIDs.PauseWindow);
                }
                else if (m_Paused) {
                    m_Paused = false;
                    Time.timeScale = m_DefaultTimeScale;
                    m_WM.ToggleWindows(WindowIDs.PauseWindow, WindowIDs.None);
                }
            }
        }
    }

    private void UpdateTime() {

        SOSaveHandler.TotalTimePlayed += Time.deltaTime;

        switch (m_CurrentLevel) {
            case 1:
                SOSaveHandler.CurrentLevel1Time += Time.deltaTime;
                break;
            case 2:
                SOSaveHandler.CurrentLevel2Time += Time.deltaTime;
                break;
            case 3:
                SOSaveHandler.CurrentLevel3Time += Time.deltaTime;
                break;
        }
    }

    private void OnNewGame(WindowIDs ignore1, WindowIDs ignore2) {
        SOSaveHandler.NewGame();
        SceneManager.LoadScene(1);
        m_InGame = true;
        m_PlayeTime = 0.0f;
        m_Paused = false;
    }

    /// <summary>
    /// LEVEL SELECT
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="level"></param>
    private void OnSelectLevel(int level) {
        SOSaveHandler.LoadLevel(level);
        SceneManager.LoadScene(level);
        Time.timeScale = m_DefaultTimeScale;
        m_InGame = true;
        m_Paused = false;
        m_PlayeTime = 0.0f;

        SOSaveHandler.CurrentLevel1Time = 0.0f;
        SOSaveHandler.CurrentLevel2Time = 0.0f;
        SOSaveHandler.CurrentLevel3Time = 0.0f;
    }

    /// <summary>
    /// PLAYER DEATH EVENT. Add to the counter for every time the player dies.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="eventArgs"></param>
    private void OnPlayerDeath() {
        SOSaveHandler.CurrentDeathCount += 1;
        SOSaveHandler.DeathCount += 1;
    }

    /// <summary>
    /// GUN EVENTS
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void OnShotgunFired() {
        SOSaveHandler.InProgressJouleShots += 1;
        SOSaveHandler.JouleShots += 1;
    }

    private void OnMachineGunFired() {
        SOSaveHandler.InProgressPersuaderShots += 1;
        SOSaveHandler.PersuaderShots += 1;
    }

    /// <summary>
    /// ENEMY DEATH EVENTS. Add to the counter for every kill the player makes.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="type"></param>
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

    private void ResetStats(WindowIDs ignore1, WindowIDs ignore2) {
        SOSaveHandler.ResetAllStats();
    }

    /// <summary>
    /// LEVEL COMPLETED EVENTS.
    /// </summary>
    /// <param name="ignore1"></param>
    /// <param name="ignore"></param>
    private void OnLevelComplete(WindowIDs ignore1, WindowIDs ignore) {
        m_IM.StopInput();
        m_InGame = false;
        Time.timeScale = 0.0f;

        switch (m_CurrentLevel) {
            case 1:
                if (SOSaveHandler.CurrentLevel1Time < SOSaveHandler.BestLevel1Time || SOSaveHandler.BestLevel1Time <= 1.0f) { SOSaveHandler.BestLevel1Time = SOSaveHandler.CurrentLevel1Time; }
                break;
            case 2:
                if (SOSaveHandler.CurrentLevel2Time < SOSaveHandler.BestLevel2Time || SOSaveHandler.BestLevel2Time <= 1.0f) { SOSaveHandler.BestLevel2Time = SOSaveHandler.CurrentLevel2Time; }
                break;
            case 3:
                if (SOSaveHandler.CurrentLevel3Time < SOSaveHandler.BestLevel3Time || SOSaveHandler.BestLevel3Time <= 1.0f) { SOSaveHandler.BestLevel3Time = SOSaveHandler.CurrentLevel3Time; }
                break;
        }

        m_PlayeTime = 0.0f;
    }

    private void OnLoadNextLevel(WindowIDs ignore1, WindowIDs ignore2) {
        SOSaveHandler.NextLevel();
        SceneManager.LoadScene(SOSaveHandler.CurrentLevel);
        m_InGame = true;
        Time.timeScale = m_DefaultTimeScale;
        m_Paused = false;
    }

    private void OnBackToMain(WindowIDs close, WindowIDs open) {

        m_InGame = false;
        m_Paused = true;

        if (SceneManager.GetActiveScene().buildIndex < 3) {
            SOSaveHandler.NextLevel();
        }
        else {
            SOSaveHandler.GameCompleted();
        }
        SceneManager.LoadScene(0);
        Time.timeScale = m_DefaultTimeScale;
    }


    /// <summary>
    /// PAUSE MENU EVENTS
    /// </summary>
    /// <param name="ignore1"></param>
    /// <param name="ignore2"></param>
    private void OnPauseContinue(WindowIDs ignore1, WindowIDs ignore2) {
        m_Paused = false;
        Time.timeScale = m_DefaultTimeScale;
    }

    private void OnPauseRestartLevel(WindowIDs ignore1, WindowIDs ignore2) {
        SOSaveHandler.RestartLevel();
        SceneManager.LoadScene(SOSaveHandler.CurrentLevel);
        m_Paused = false;
        Time.timeScale = m_DefaultTimeScale;

        m_PlayeTime = 0.0f;
    }

    private void OnPauseBackToMain(WindowIDs ignore1, WindowIDs ignore2) {
        m_Paused = true;
        SceneManager.LoadScene(0);
        Time.timeScale = m_DefaultTimeScale;
        m_InGame = false;

    }

    /// <summary>
    /// Things to do when a level is loaded.
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {

        m_CurrentLevel = scene.buildIndex;

        if (scene.buildIndex != 0) {
            m_InGame = true;
            m_PlayeTime = 0.0f;

            if (SOSaveHandler.CheckpointID == 0) {
                GameObject player = SpawnPlayer();

                if (scene.buildIndex == 1) {
                    player.GetComponentInChildren<WeaponSelect>().MGAvailable = true;
                    player.GetComponentInChildren<WeaponSelect>().SGAvailable = false;
                    m_IM.StopInput();
                }
            }
            else {
                SpawnPlayer();
                GameObject[] checkpoints = GameObject.FindGameObjectsWithTag(StringConstantUtility.CHECKPOINT);
                GameObject camera = GameObject.FindGameObjectWithTag(StringConstantUtility.SMART_CAMERA_TAG);

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
        else if (scene.buildIndex == 0) {
            m_InGame = false;
        }
    }

    private GameObject SpawnPlayer() {
        GameObject _player = Instantiate(Resources.Load(MAIN_CHARACTER_PATH, typeof(GameObject)) as GameObject, transform.position, Quaternion.identity) as GameObject;
        SOEffectHandler.PlayEffect(EffectEnums.PlayerRespawn, SOSaveHandler.CheckpointPosition);
        _player.transform.position = SOSaveHandler.CheckpointPosition;
        m_IM.AssignPlayer(_player);
        if (SOSaveHandler.CurrentLevel > 1) { _player.GetComponentInChildren<WeaponSelect>().SGAvailable = true; }
        else if (SOSaveHandler.CurrentLevel == 1 && SOSaveHandler.CheckpointID >= 7 && SOSaveHandler.CheckpointID != 8) { _player.GetComponentInChildren<WeaponSelect>().SGAvailable = true; }
        else { _player.GetComponentInChildren<WeaponSelect>().SGAvailable = false; }
        return _player;
    }

    #endregion Private Methods
}
