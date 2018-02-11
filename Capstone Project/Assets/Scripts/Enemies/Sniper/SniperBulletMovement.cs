using UnityEngine;

public class SniperBulletMovement : MonoBehaviour {

    #region Constant Fields
    private const float Y_OFFSET = 7.5f;

    #endregion Constnat Fields

    #region Private Fields
    [SerializeField]
    private float _speed;

    private Vector3 m_playerPos = Vector3.zero;
    private bool m_hasFired = false;

    #endregion Private Fields

    #region Private Initializers
    private void OnEnable() {

        // Get the center of the player based on the collider
        m_playerPos = GameObject.FindGameObjectWithTag(StringConstantUtility.PLAYER_TAG).GetComponent<BoxCollider2D>().bounds.center;
        // Offset where the bullet will go by 7.5 in the y-axis
        m_playerPos = new Vector3(m_playerPos.x, m_playerPos.y + Y_OFFSET, 0.0f);
    }

    #endregion Private Initializers

    #region Private Mathods
    private void Update() {
        
        if (!m_hasFired) {

            m_hasFired = true;

            Vector3 unitVect = (m_playerPos - transform.position).normalized;
            GetComponent<Rigidbody2D>().AddForce(unitVect * _speed, ForceMode2D.Impulse);
        }
    }

    #endregion Private Methods
}
