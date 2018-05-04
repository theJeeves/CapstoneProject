using System;
using UnityEngine;

public class AbstractEnemyDealDamage : MonoBehaviour {

    #region Protected Fields
    [SerializeField]
    protected int _damage;

    #endregion Protected Fields

    #region Events
    public static event EventHandler<int> DecrementHealth;

    #endregion Events

    #region Protected Methods
    protected virtual void OnTriggerEnter2D(Collider2D collider) {

        if (collider.gameObject.tag == StringConstantUtility.PLAYER_TAG) {

            DecrementHealth?.Invoke(this, _damage);
        }
    }

    #endregion Protected Methods
}
