using UnityEngine;

public class spikes : MonoBehaviour {

    #region Constant Fields
    private const string PLAYER_TAG = "Player";
    #endregion Constant Fields

    #region Fields
    [SerializeField]
    protected int _damage;
    #endregion Fields

    #region Private Methods
    private void OnCollisionEnter2D(Collision2D otherGO)
    {
        string tag = otherGO.gameObject.tag;

        if (tag == PLAYER_TAG) {
            otherGO.gameObject.GetComponent<PlayerHealth>().KillPlayer();
        }
    }
    #endregion Private Methods
}
