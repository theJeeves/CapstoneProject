using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager> {

    public SOSaveFile SOSaveHandler;
    public SOEffects SOEffectHandler;

    private GameObject _player;
    private bool _inGame = false;

    protected override void Awake() {
        // CALL BASE AWAKE TO ENSURE THERE ARE NO OTHER INSTANCES OF THE GAME MANAGER IN THE SCENE
        base.Awake();

        // LOAD THE SCRIPTABLEOBJECTS TO BE USED THROUGHOUT THE GAME.
        SOSaveHandler = Resources.Load("ScriptableObjects/PlayerSaveFile", typeof(SOSaveFile)) as SOSaveFile;
        SOEffectHandler = Resources.Load("ScriptableObjects/SOEffectHandler", typeof(SOEffects)) as SOEffects;

        SOEffectHandler.LoadEffects();
    }

    private void OnEnable() {
        // Start Window Events
        StartWindow.OnContinue += OnContinue;
        StartWindow.OnNewGame += OnNewGame;

        // Player Death Event
        PlayerHealth.OnPlayerDeath += OnPlayerDeath;

        // Gun Events
        Shotgun.ShotFired += OnShotgunFired;
        MachineGun.ShotFired += OnMachineGunFired;

        // Enemy Death Events
        EnemyBasicBehaviors.OnDeath += OnEnemyDeath;
    }

    private void OnDisable() {
        // Start Window Events
        StartWindow.OnContinue -= OnContinue;
        StartWindow.OnNewGame -= OnNewGame;

        // Player Death Event
        PlayerHealth.OnPlayerDeath -= OnPlayerDeath;

        // Gun Events
        Shotgun.ShotFired -= OnShotgunFired;
        MachineGun.ShotFired -= OnMachineGunFired;

        // Enemy Death Events
        EnemyBasicBehaviors.OnDeath -= OnEnemyDeath;
    }

    private void OnContinue(WindowIDs ignore1, WindowIDs ignore2) {
        SceneManager.LoadScene(SOSaveHandler.CurrentLevel);
    }

    private void OnNewGame(WindowIDs ignore1, WindowIDs ignore2) {
        SOSaveHandler.NewGame();
        SceneManager.LoadScene(1);
    }

    private void OnLevelWasLoaded(int level) {
        if (level == 1) {
            if (SOSaveHandler.CheckpointID == 0) {
                SpawnPlayer();
            }
            else {
                SpawnPlayer();
                GameObject[] checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
                foreach(GameObject ckpt in checkpoints) {

                    if (ckpt.GetComponent<CheckpointBehavior>().ID == SOSaveHandler.CheckpointID) {
                        ckpt.GetComponent<CheckpointBehavior>().Activate();
                    }
                }
            }
        }
        else if (level == 2) {

        }
    }

    private void SpawnPlayer() {
        GameObject _player = Instantiate(Resources.Load("MainCharacter/MainCharacter", typeof(GameObject)) as GameObject, transform.position, Quaternion.identity) as GameObject;
        SOEffectHandler.PlayEffect(EffectEnums.PlayerRespawn, SOSaveHandler.CheckpointPosition);
        _player.transform.position = SOSaveHandler.CheckpointPosition;
    }

    // Add to the counter for every time the player dies
    private void OnPlayerDeath() {
        SOSaveHandler.DeathCount += 1;
    }

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

    private void OnShotgunFired() {
        SOSaveHandler.JouleShots += 1;
    }

    private void OnMachineGunFired() {
        SOSaveHandler.PersuaderShots += 1;
    }
}
