using UnityEngine;
using UnityEngine.Events;

public class SniperPushBack : MonoBehaviour {

    #region Constant
    private const float FORCE_MULTIPLIER = 25000.0f;

    #endregion Constants

    #region Private Fields
    private Rigidbody2D m_Body2d;
    private float m_Direction;

    #endregion Private Fields


    #region Events
    public static event UnityAction Stun;

    #endregion Events

    #region Private Methods
    private void OnTriggerEnter2D(Collider2D go) {

        if (go.gameObject.tag == Tags.PlayerTag) {

            Stun?.Invoke();
            m_Body2d = go.GetComponent<Rigidbody2D>();
            m_Direction = transform.localScale.x > 0 ? 1.0f : -1.0f;
            m_Body2d.AddForce(new Vector2(1.0f, 0.0f) * FORCE_MULTIPLIER * m_Direction, ForceMode2D.Impulse);
        }
    }

    #endregion Private Methods
}
