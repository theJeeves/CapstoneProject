using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName ="SO Checkpoint/Create SO Checkpoint")]
public class SOCheckpoint : ScriptableObject {

    public Vector2 checkpointPosition;
    public GameObject checkpointGO;
}
