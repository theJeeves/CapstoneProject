using UnityEngine;

public class SniperPushBack : MonoBehaviour {

    #region Constant
    private const float FORCE_MULTIPLIER = 25000.0f;

    #endregion Constants

    #region Private Fields
    private Rigidbody2D _body2d;
    private float _direction;

    #endregion Private Fields

    #region Delegates
    public delegate void SniperPushBackEvent();

    #endregion Delegates

    #region Events
    public static event SniperPushBackEvent Stun;

    #endregion Events

    #region Private Methods
    private void OnTriggerEnter2D(Collider2D go) {

        if (go.gameObject.tag == StringConstantUtility.PLAYER_TAG) {

            Stun?.Invoke();
            _body2d = go.GetComponent<Rigidbody2D>();
            _direction = transform.localScale.x > 0 ? 1.0f : -1.0f;
            _body2d.AddForce(new Vector2(1.0f, 0.0f) * FORCE_MULTIPLIER * _direction, ForceMode2D.Impulse);
        }
    }

    #endregion Private Methods
}
