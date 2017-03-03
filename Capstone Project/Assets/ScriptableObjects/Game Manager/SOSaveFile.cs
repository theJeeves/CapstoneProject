using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName ="SO Save File/New SO Save File")]
public class SOSaveFile : ScriptableObject {

    public SOCheckpoint SOCheckpointHandler;
    public int currentLevel = 1;

    public void NewGame() {
       SOCheckpointHandler.checkpointGO = null;
        SOCheckpointHandler.checkpointPosition = new Vector3(-5350.0f, 95.0f, 0.0f);
        SOCheckpointHandler.checkPointReached = false;
        currentLevel = 1;
    }
}
