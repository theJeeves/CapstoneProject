using UnityEngine;
using System.Collections;

public class PlayerBulletMovement : MonoBehaviour {

    #region Constant Fields
    private const string BARREL_TAG = "Barrle";
    private const string DIRECTION_TAG = "Direction";
    #endregion Constant Fields

    #region Fields
    [SerializeField]
    private float m_Speed;

    private Vector2 m_Barrel;
    private Vector2 m_Target;
    private Vector3 m_Direction;
    #endregion Fields

    #region Initializers
    private void Start()
    {
        m_Barrel = GameObject.FindGameObjectWithTag(BARREL_TAG).transform.position;
        m_Target = GameObject.FindGameObjectWithTag(DIRECTION_TAG).transform.position;

        m_Direction = (m_Target - m_Barrel).normalized;

        transform.localEulerAngles = new Vector3(transform.localRotation.x, transform.localRotation.y, Vector3.Angle(m_Direction, Vector3.up));

        StartCoroutine(Movement());
    }

    #endregion Initializers

    #region Private Methods
    private IEnumerator Movement()
    {
        while (true) {
            transform.position = Vector2.MoveTowards(transform.position, transform.position + (m_Direction * m_Speed), (m_Speed * Time.deltaTime));

            yield return 0;
        }
    }
    #endregion Private Methods
}
