using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName ="SO Checkpoint/Create SO Checkpoint")]
public class SOCheckpoint : ScriptableObject {

    public Vector2 checkpointPosition;          // Keeps track of where the player hit the checkpoint
    public GameObject checkpointGO;             // GameObject to determine if the player has reached the same
                                                // checkpoint or a new one.
}
