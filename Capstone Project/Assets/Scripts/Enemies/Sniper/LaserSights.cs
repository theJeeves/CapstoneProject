using UnityEngine;

public class LaserSights : MonoBehaviour {

    #region Constant Fields
    private const float Y_OFFSET = 7.5f;
    private const float LINE_RENDER_WIDTH = 2.5f;

    #endregion Constant Fields

    #region Private Fields
    [SerializeField]
    private SOEffects _SOEffectHandler;
    [SerializeField]
    private LayerMask _whatToHit;
    [SerializeField]
    private Transform _endOfBarrel;

    private BoxCollider2D m_PlayerBox = null;
    private Vector3 m_PlayerPos = Vector3.zero;
    private Vector2 m_Obstruction = Vector2.zero;
    private LineRenderer m_Renderer = null;
    private GameObject m_LaserEffect = null;

    #endregion Private Fields

    #region Initializers
    private void Awake() {
        m_Renderer = GetComponent<LineRenderer>();
    }

    private void OnEnable() {
        m_LaserEffect = _SOEffectHandler.PlayEffect(EffectEnums.SniperLaserEffect, _endOfBarrel.position);
    }

    #endregion Initializers

    #region Finalizers
    private void OnDisable() {
        _SOEffectHandler.StopEffect(m_LaserEffect);
    }

    #endregion Finalizers

    #region Private Methods
    private void Update() {

        if (m_PlayerBox == null) {
            m_PlayerBox = GameObject.FindGameObjectWithTag(StringConstantUtility.PLAYER_TAG).GetComponent<BoxCollider2D>();
        }

        m_PlayerPos = new Vector2(m_PlayerBox.bounds.center.x, m_PlayerBox.bounds.center.y + Y_OFFSET);

        CheckCollisions();

        //Set the Start Position
        m_Renderer.SetPosition(0, _endOfBarrel.position);

        //Set the End Position
        if (m_Obstruction == Vector2.zero) {
            m_Renderer.SetPosition(1, m_PlayerPos);
            m_LaserEffect.transform.position = m_PlayerPos;
        }
        else {
            m_Renderer.SetPosition(1, m_Obstruction);
            m_LaserEffect.transform.position = m_Obstruction;
        }

        //Set the width of the line renderer
        m_Renderer.startWidth = LINE_RENDER_WIDTH;
        m_Renderer.endWidth = LINE_RENDER_WIDTH;
        m_Renderer.startColor = Color.red;
        m_Renderer.endColor = Color.red;
    }

    private void CheckCollisions() {

        // Send out a raycast to determine if the laser should be touching the player or a world object.
        RaycastHit2D hit = Physics2D.Raycast(_endOfBarrel.position, m_PlayerPos - _endOfBarrel.position, 
            Vector3.Magnitude(m_PlayerPos - _endOfBarrel.position), _whatToHit);

        if (hit.collider != null) {
            m_Obstruction = hit.collider.gameObject.tag == StringConstantUtility.PLAYER_TAG ? Vector2.zero : hit.point;
        }
    }

    #endregion Private Methods
}
