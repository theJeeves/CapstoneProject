using UnityEngine;
using System.Collections;

public class EndOfLevel : MonoBehaviour {

    public delegate void EndOfLevelEvent(WindowIDs close, WindowIDs open);
    public static event EndOfLevelEvent OnLevelComplete;

    [SerializeField]
    private int _nextLevel = -1;

    private void OnTriggerEnter2D(Collider2D otherGO) {

        if (otherGO.tag == "Player") {
            if (OnLevelComplete != null) { OnLevelComplete(WindowIDs.None, WindowIDs.EndOfLevelWindow); }
        }
    }
}
