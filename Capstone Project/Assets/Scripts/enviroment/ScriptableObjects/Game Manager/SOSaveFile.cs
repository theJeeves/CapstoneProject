using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName ="SO Save File/New SO Save File")]
public class SOSaveFile : ScriptableObject {

    [SerializeField]
    private Vector3[] _startingPositions;

    public int CurrentLevel {
       get { return PlayerPrefs.GetInt("currentLevel", 1); }
        set { PlayerPrefs.SetInt("currentLevel", value); }
    }

    public int CheckpointID {
        get { return PlayerPrefs.GetInt("checkpointID", 0); }
        set { PlayerPrefs.SetInt("checkpointID", value); }
    }

    public Vector3 CheckpointPosition {
        get {
            return new Vector3( PlayerPrefs.GetFloat("checkpointPositionX", _startingPositions[0].x), 
            PlayerPrefs.GetFloat("checkpointPositionY", _startingPositions[0].y), 0.0f); }
        set {
            PlayerPrefs.SetFloat("checkpointPositionX", value.x);
            PlayerPrefs.SetFloat("checkpointPositionY", value.y);
        }
    }

    public Vector3 CameraPosition {
        get {
            return new Vector3(PlayerPrefs.GetFloat("cameraPositionX"), 
            PlayerPrefs.GetFloat("cameraPositionY"), 
            PlayerPrefs.GetFloat("cameraPositionZ") );
        }
        set {
            PlayerPrefs.SetFloat("cameraPositionX", value.x);
            PlayerPrefs.SetFloat("cameraPositionY", value.y);
            PlayerPrefs.SetFloat("cameraPositionZ", value.z);
        }
    }

    public Vector2 SmartCameraSettings {
        get { return new Vector2(PlayerPrefs.GetInt("smartX"), PlayerPrefs.GetInt("smartY")); }
        set {
            PlayerPrefs.SetFloat("smartX", value.x);
            PlayerPrefs.SetFloat("smartY", value.y);
        }
    }

    public int DeathCount {
        get { return PlayerPrefs.GetInt("deathCount", 0); }
        set { PlayerPrefs.SetInt("deathCount", value);}
    }

    public int CurrentDeathCount {
        get { return PlayerPrefs.GetInt("currentDeathCount", 0); }
        set { PlayerPrefs.SetInt("currentDeathCount", value); }
    }

    public int PersuaderShots {
        get { return PlayerPrefs.GetInt("persuaderShots", 0); }
        set { PlayerPrefs.SetInt("persuaderShots", value); }
    }

    public int InProgressPersuaderShots {
        get { return PlayerPrefs.GetInt("inprogresspersuaderShots", 0); }
        set { PlayerPrefs.SetInt("inprogresspersuaderShots", value); }
    }

    public int JouleShots {
        get { return PlayerPrefs.GetInt("jouleShots", 0); }
        set { PlayerPrefs.SetInt("jouleShots", value); }
    }
    public int InProgressJouleShots {
        get { return PlayerPrefs.GetInt("inprogressjouleShots", 0); }
        set { PlayerPrefs.SetInt("inprogressjouleShots", value); }
    }

    public int TotalShotsFired {
        get { return PlayerPrefs.GetInt("persuaderShots", 0) + PlayerPrefs.GetInt("jouleShots", 0); }
    }

    public int AcidVectorsKilled {
        get { return PlayerPrefs.GetInt("acidVectorsKilled", 0); }
        set { PlayerPrefs.SetInt("acidVectorsKilled", value); }
    }

    public int ExplosiveVectorsKilled {
        get { return PlayerPrefs.GetInt("explosiveVectorsKilled", 0); }
        set { PlayerPrefs.SetInt("explosiveVectorsKilled", value); }
    }

    public int FlyingVectorsKilled {
        get { return PlayerPrefs.GetInt("flyingVectorsKilled", 0); }
        set { PlayerPrefs.SetInt("flyingVectorsKilled", value); }
    }

    public int SnipersKilled {
        get { return PlayerPrefs.GetInt("snipersKilled", 0); }
        set { PlayerPrefs.SetInt("snipersKilled", value); }
    }

    public int ChargersKilled {
        get { return PlayerPrefs.GetInt("chargersKilled", 0); }
        set { PlayerPrefs.SetInt("chargersKilled", value); }
    }

    public int TotalEnemiesKilled {
        get { return PlayerPrefs.GetInt("acidVectorsKilled", 0) + PlayerPrefs.GetInt("explosiveVectorsKilled", 0) + PlayerPrefs.GetInt("flyingVectorsKilled", 0) +
                PlayerPrefs.GetInt("snipersKilled", 0) + PlayerPrefs.GetInt("chargersKilled", 0);
        }
    }

    public int JouleEnabled {
        get { return PlayerPrefs.GetInt("jouleEnabled", 0); }
        set { PlayerPrefs.SetInt("jouleEnabled", value); }
    }

    public float CurrentLevel1Time {
        get { return PlayerPrefs.GetFloat("currentLevel1Time", 0.0f); }
        set { PlayerPrefs.SetFloat("currentLevel1Time", value); }
    }

    public float CurrentLevel2Time {
        get { return PlayerPrefs.GetFloat("currentLevel2Time", 0.0f); }
        set { PlayerPrefs.SetFloat("currentLevel2Time", value); }
    }

    public float CurrentLevel3Time {
        get { return PlayerPrefs.GetFloat("currentLevel3Time", 0.0f); }
        set { PlayerPrefs.SetFloat("currentLevel3Time", value); }
    }

    public float BestLevel1Time {
        get { return PlayerPrefs.GetFloat("bestLevel1Time", 0.0f); }
        set { PlayerPrefs.SetFloat("bestLevel1Time", value); }
    }

    public float BestLevel2Time {
        get { return PlayerPrefs.GetFloat("bestLevel2Time", 0.0f); }
        set { PlayerPrefs.SetFloat("bestLevel2Time", value); }
    }

    public float BestLevel3Time {
        get { return PlayerPrefs.GetFloat("bestLevel3Time", 0.0f); }
        set { PlayerPrefs.SetFloat("bestLevel3Time", value); }
    }

    public float TotalTimePlayed {
        get { return PlayerPrefs.GetFloat("totalTimePlayed", 0.0f); }
        set { PlayerPrefs.SetFloat("totalTimePlayed", value); }
    }

    public void NewGame() {
        PlayerPrefs.DeleteAll();
    }

    public void NextLevel() {
        CurrentLevel += 1;
        CheckpointID = 0;
        CheckpointPosition = _startingPositions[CurrentLevel - 1];
        CurrentDeathCount = 0;
        InProgressPersuaderShots = 0;
        InProgressJouleShots = 0;

        CurrentLevel1Time = 0.0f;
        CurrentLevel2Time = 0.0f;
        CurrentLevel3Time = 0.0f;
    }

    public void RestartLevel() {
        CheckpointID = 0;
        CheckpointPosition = _startingPositions[CurrentLevel - 1];
        CurrentDeathCount = 0;
        InProgressPersuaderShots = 0;
        InProgressJouleShots = 0;

        CurrentLevel1Time = 0.0f;
        CurrentLevel2Time = 0.0f;
        CurrentLevel3Time = 0.0f;
    }

    public void LoadLevel(int level) {
        CheckpointID = 0;
        CurrentLevel = level;
        CheckpointPosition = _startingPositions[CurrentLevel - 1];
        CurrentDeathCount = 0;
        InProgressPersuaderShots = 0;
        InProgressJouleShots = 0;
    }

    public void ResetAllStats() {

        NewGame();
    }

    public void GameCompleted() {
        CurrentLevel = 1;
        CheckpointID = 0;
        CheckpointPosition = _startingPositions[0];


        CurrentLevel1Time = 0.0f;
        CurrentLevel2Time = 0.0f;
        CurrentLevel3Time = 0.0f;
    }
}
