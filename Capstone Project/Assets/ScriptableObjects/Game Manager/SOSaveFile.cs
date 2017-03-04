using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName ="SO Save File/New SO Save File")]
public class SOSaveFile : ScriptableObject {

    public int currentLevel;
    public int checkpointID;
    public Vector2 checkpointPosition;

    public void NewGame() {
        currentLevel = 1;
        checkpointID = 0;
        checkpointPosition = new Vector3(-5350.0f, 95.0f, 0.0f);
    }
}
