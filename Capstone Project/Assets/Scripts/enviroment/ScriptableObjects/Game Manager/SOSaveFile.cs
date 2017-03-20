using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName ="SO Save File/New SO Save File")]
public class SOSaveFile : ScriptableObject {

    [SerializeField]
    private Vector3[] _startingPositions;
    //[SerializeField]
    //private Vector3[] _startingCameraPos;

    public int CurrentLevel {
       get { return PlayerPrefs.GetInt("currentLevel"); }
        set { PlayerPrefs.SetInt("currentLevel", value); }
    }

    public int CheckpointID {
        get { return PlayerPrefs.GetInt("checkpointID"); }
        set { PlayerPrefs.SetInt("checkpointID", value); }
    }

    public Vector3 CheckpointPosition {
        get { return new Vector3( PlayerPrefs.GetFloat("checkpointPositionX"), PlayerPrefs.GetFloat("checkpointPositionY"), 0.0f); }
        set {
            PlayerPrefs.SetFloat("checkpointPositionX", value.x);
            PlayerPrefs.SetFloat("checkpointPositionY", value.y);
        }
    }

    public Vector3 CameraPosition {
        get { return new Vector3(PlayerPrefs.GetFloat("cameraPositionX"), PlayerPrefs.GetFloat("cameraPositionY"), PlayerPrefs.GetFloat("cameraPositionZ")); }
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
        get { return PlayerPrefs.GetInt("deathCount"); }
        set { PlayerPrefs.SetInt("deathCount", value);}
    }

    public int CurrentDeathCount {
        get { return PlayerPrefs.GetInt("currentDeathCount"); }
        set { PlayerPrefs.SetInt("currentDeathCount", value); }
    }

    public int PersuaderShots {
        get { return PlayerPrefs.GetInt("persuaderShots"); }
        set { PlayerPrefs.SetInt("persuaderShots", value); }
    }

    public int InProgressPersuaderShots {
        get { return PlayerPrefs.GetInt("inprogresspersuaderShots"); }
        set { PlayerPrefs.SetInt("inprogresspersuaderShots", value); }
    }

    public int JouleShots {
        get { return PlayerPrefs.GetInt("jouleShots"); }
        set { PlayerPrefs.SetInt("jouleShots", value); }
    }
    public int InProgressJouleShots {
        get { return PlayerPrefs.GetInt("inprogressjouleShots"); }
        set { PlayerPrefs.SetInt("inprogressjouleShots", value); }
    }

    public int TotalShotsFired {
        get { return PlayerPrefs.GetInt("persuaderShots") + PlayerPrefs.GetInt("jouleShots"); }
    }

    public int AcidVectorsKilled {
        get { return PlayerPrefs.GetInt("acidVectorsKilled"); }
        set { PlayerPrefs.SetInt("acidVectorsKilled", value); }
    }

    public int ExplosiveVectorsKilled {
        get { return PlayerPrefs.GetInt("explosiveVectorsKilled"); }
        set { PlayerPrefs.SetInt("explosiveVectorsKilled", value); }
    }

    public int FlyingVectorsKilled {
        get { return PlayerPrefs.GetInt("flyingVectorsKilled"); }
        set { PlayerPrefs.SetInt("flyingVectorsKilled", value); }
    }

    public int SnipersKilled {
        get { return PlayerPrefs.GetInt("snipersKilled"); }
        set { PlayerPrefs.SetInt("snipersKilled", value); }
    }

    public int ChargersKilled {
        get { return PlayerPrefs.GetInt("chargersKilled"); }
        set { PlayerPrefs.SetInt("chargersKilled", value); }
    }

    public int TotalEnemiesKilled {
        get { return PlayerPrefs.GetInt("acidVectorsKilled") + PlayerPrefs.GetInt("explosiveVectorsKilled") + PlayerPrefs.GetInt("flyingVectorsKilled") +
                PlayerPrefs.GetInt("snipersKilled") + PlayerPrefs.GetInt("chargersKilled");
        }
    }

    public int JouleEnabled {
        get { return PlayerPrefs.GetInt("jouleEnabled"); }
        set { PlayerPrefs.SetInt("jouleEnabled", value); }
    }

    public float CurrentLevel1Time {
        get { return PlayerPrefs.GetFloat("currentLevel1Time"); }
        set { PlayerPrefs.SetFloat("currentLevel1Time", value); }
    }

    public float CurrentLevel2Time {
        get { return PlayerPrefs.GetFloat("currentLevel2Time"); }
        set { PlayerPrefs.SetFloat("currentLevel2Time", value); }
    }

    public float CurrentLevel3Time {
        get { return PlayerPrefs.GetFloat("currentLevel3Time"); }
        set { PlayerPrefs.SetFloat("currentLevel3Time", value); }
    }

    public float BestLevel1Time {
        get { return PlayerPrefs.GetFloat("bestLevel1Time"); }
        set { PlayerPrefs.SetFloat("bestLevel1Time", value); }
    }

    public float BestLevel2Time {
        get { return PlayerPrefs.GetFloat("bestLevel2Time"); }
        set { PlayerPrefs.SetFloat("bestLevel2Time", value); }
    }

    public float BestLevel3Time {
        get { return PlayerPrefs.GetFloat("bestLevel3Time"); }
        set { PlayerPrefs.SetFloat("bestLevel3Time", value); }
    }

    public float TotalTimePlayed {
        get { return PlayerPrefs.GetFloat("totalTimePlayed"); }
        set { PlayerPrefs.SetFloat("totalTimePlayed", value); }
    }

    public void NewGame() {
        CurrentLevel = 1;
        CheckpointID = 0;
        CheckpointPosition = _startingPositions[0];
        DeathCount = 0;
        CurrentDeathCount = 0;
        PersuaderShots = 0;
        InProgressPersuaderShots = 0;
        JouleShots = 0;
        InProgressJouleShots = 0;
        AcidVectorsKilled = 0;
        ExplosiveVectorsKilled = 0;
        FlyingVectorsKilled = 0;
        SnipersKilled = 0;
        ChargersKilled = 0;
        JouleEnabled = 0;

        CurrentLevel1Time = 0.0f;
        CurrentLevel2Time = 0.0f;
        CurrentLevel3Time = 0.0f;
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

        CurrentLevel = 1;
        CheckpointID = 0;
        CheckpointPosition = _startingPositions[0];

        DeathCount = 0;
        CurrentDeathCount = 0;
        PersuaderShots = 0;
        InProgressPersuaderShots = 0;
        JouleShots = 0;
        InProgressJouleShots = 0;
        AcidVectorsKilled = 0;
        ExplosiveVectorsKilled = 0;
        FlyingVectorsKilled = 0;
        SnipersKilled = 0;
        ChargersKilled = 0;
        JouleEnabled = 0;

        TotalTimePlayed = 0.0f;

        CurrentLevel1Time = 0.0f;
        CurrentLevel2Time = 0.0f;
        CurrentLevel3Time = 0.0f;

        BestLevel1Time = 0.0f;
        BestLevel2Time = 0.0f;
        BestLevel3Time = 0.0f;
    }
}
