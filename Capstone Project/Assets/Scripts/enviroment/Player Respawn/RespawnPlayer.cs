using UnityEngine;

public class RespawnPlayer : MonoBehaviour {

    #region Private Methods
    private void OnTriggerEnter2D(Collider2D otherGO) {

        if (otherGO.tag == Tags.PlayerTag) {
            otherGO.gameObject.GetComponent<PlayerHealth>().KillPlayer();
        }
    }

    #endregion Private Methods
}
