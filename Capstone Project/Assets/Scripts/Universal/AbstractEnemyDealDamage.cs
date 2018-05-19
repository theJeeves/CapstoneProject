using UnityEngine;
using UnityEngine.Events;

public class AbstractEnemyDealDamage : MonoBehaviour {

    #region Protected Fields
    [SerializeField]
    protected int _damage;

    #endregion Protected Fields

    #region Events
    public static event UnityAction<int> DecrementHealth;

    #endregion Events

    #region Protected Methods
    protected virtual void OnTriggerEnter2D(Collider2D collider) {

        if (collider.gameObject.tag == Tags.PlayerTag) {

            DecrementHealth?.Invoke(_damage);
        }
    }

    #endregion Protected Methods
}
