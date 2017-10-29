using UnityEngine;
using System.Collections;

public class SniperBulletMovement : MonoBehaviour {

    [SerializeField]
    private float _speed;

    private Vector3 m_playerPos = Vector3.zero;
    private bool m_hasFired = false;

    private void OnEnable() {

        // Get the center of the player based on the collider
        m_playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<BoxCollider2D>().bounds.center;
        // Offset where the bullet will go by 7.5 in the y-axis
        m_playerPos = new Vector3(m_playerPos.x, m_playerPos.y + 7.5f, 0.0f);
    }

    private void Update() {
        
        if (!m_hasFired) {

            m_hasFired = true;

            Vector3 unitVect = (m_playerPos - transform.position).normalized;
            GetComponent<Rigidbody2D>().AddForce(unitVect * _speed, ForceMode2D.Impulse);
        }
    }
}
