using UnityEngine;

public class EndOfLevel : MonoBehaviour {

    #region Delegates
    public delegate void EndOfLevelEvent(WindowIDs close, WindowIDs open);

    #endregion Delegates

    #region Events
    public static event EndOfLevelEvent LevelComplete;

    #endregion Events

    #region Private Methods
    private void OnTriggerEnter2D(Collider2D otherGO) {

        if (otherGO.tag == StringConstantUtility.PLAYER_TAG) {
            LevelComplete?.Invoke(WindowIDs.None, WindowIDs.EndOfLevelWindow);
        }
    }

    #endregion Private Methods
}
