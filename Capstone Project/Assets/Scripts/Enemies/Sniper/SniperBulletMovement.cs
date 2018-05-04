using UnityEngine;

public class SniperBulletMovement : MonoBehaviour {

    #region Constant Fields
    private const float Y_OFFSET = 7.5f;

    #endregion Constnat Fields

    #region Private Fields
    [SerializeField]
    private float _speed;

    private Vector3 m_PlayerPos = Vector3.zero;
    private bool m_HasFired = false;

    #endregion Private Fields

    #region Initializers
    private void OnEnable() {

        // Get the center of the player based on the collider
        m_PlayerPos = GameObject.FindGameObjectWithTag(StringConstantUtility.PLAYER_TAG).GetComponent<BoxCollider2D>().bounds.center;
        // Offset where the bullet will go by 7.5 in the y-axis
        m_PlayerPos = new Vector3(m_PlayerPos.x, m_PlayerPos.y + Y_OFFSET, 0.0f);
    }

    #endregion Initializers

    #region Private Mathods
    private void Update() {
        
        if (!m_HasFired) {

            m_HasFired = true;

            Vector3 unitVect = (m_PlayerPos - transform.position).normalized;
            GetComponent<Rigidbody2D>().AddForce(unitVect * _speed, ForceMode2D.Impulse);
        }
    }

    #endregion Private Methods
}
