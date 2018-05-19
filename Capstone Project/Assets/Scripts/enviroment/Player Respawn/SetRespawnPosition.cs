using UnityEngine;

public class SetRespawnPosition : MonoBehaviour {

    #region Private Fields
    [SerializeField]
    private SORespawn _respawnContainer;

    #endregion Private Fields

    #region Private Methods
    private void OnTriggerEnter2D(Collider2D otherGO) {

        if (otherGO.tag == Tags.PlayerTag) {
            _respawnContainer.respawnPos = transform.position;
        }
    }

    #endregion Private Methods
}
