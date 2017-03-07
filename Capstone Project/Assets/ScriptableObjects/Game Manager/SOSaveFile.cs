using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName ="SO Save File/New SO Save File")]
public class SOSaveFile : ScriptableObject {

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

    public int DeathCount {
        get { return PlayerPrefs.GetInt("deathCount"); }
        set { PlayerPrefs.SetInt("deathCount", value); }
    }

    public int PersuaderShots {
        get { return PlayerPrefs.GetInt("persuaderShots"); }
        set { PlayerPrefs.SetInt("persuaderShots", value); }
    }

    public int JouleShots {
        get { return PlayerPrefs.GetInt("jouleShots"); }
        set { PlayerPrefs.SetInt("jouleShots", value); }
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

    public void NewGame() {
        CurrentLevel = 1;
        CheckpointID = 0;
        CheckpointPosition = new Vector3(-5350.0f, 95.0f, 0.0f);
        DeathCount = 0;
        PersuaderShots = 0;
        JouleShots = 0;
        AcidVectorsKilled = 0;
        ExplosiveVectorsKilled = 0;
        FlyingVectorsKilled = 0;
        SnipersKilled = 0;
        ChargersKilled = 0;
    }
}
