using UnityEngine;

public class RespawnPlayer : MonoBehaviour {

    #region Private Methods
    private void OnTriggerEnter2D(Collider2D otherGO) {

        if (otherGO.tag == StringConstantUtility.PLAYER_TAG) {
            otherGO.gameObject.GetComponent<PlayerHealth>().KillPlayer();
        }
    }

    #endregion Private Methods
}
