using UnityEngine;

public class spikes : MonoBehaviour {

    #region Protected Fields
    [SerializeField]
    protected int _damage;

    #endregion Protected Fields

    #region Private Methods
    private void OnCollisionEnter2D(Collision2D otherGO)
    {
        if (otherGO.gameObject.tag == Tags.PlayerTag) {
            otherGO.gameObject.GetComponent<PlayerHealth>().KillPlayer();
        }
    }

    #endregion Private Methods
}
