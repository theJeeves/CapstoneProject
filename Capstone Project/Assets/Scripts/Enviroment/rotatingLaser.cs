using UnityEngine;

public class rotatingLaser : MonoBehaviour
{
    #region Constants
    private const float LASER_START_WIDTH = 2.5f;
    private const float LASER_END_WIDTH = 2.5f;
    private const int MIN_COLOR_INDEX = 0;
    private const int MAX_COLOR_INDEX = 5;

    #endregion Constants

    #region Private Fields
    [SerializeField]
    private GameObject endBarrel;
    [SerializeField]
    private GameObject startBarrel;
    [SerializeField]
    private int _damage;
    [SerializeField]
    private LayerMask _whatToHit;

    private Vector2 m_Direction = Vector2.zero;
    private LineRenderer m_LineRenderer = null;
    private Color[] m_ColorArray = { Color.blue, Color.cyan, Color.green, Color.magenta, Color.red, Color.yellow };

    #endregion Private Fields


    #region Initializers
    // Use this for initialization
    void Start()
    {
        m_LineRenderer = GetComponentInChildren<LineRenderer>();
    }

    #endregion Initializers

    #region Private Methods
    // Update is called once per frame
    private void Update()
    {
        m_Direction = endBarrel.transform.position - startBarrel.transform.position;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, m_Direction, Mathf.Infinity, _whatToHit);
        if (hit.collider != null)
        {
            Vector2 laserEnd = new Vector2(hit.point.x, hit.point.y);
            m_LineRenderer.startWidth = LASER_START_WIDTH;
            m_LineRenderer.endWidth = LASER_END_WIDTH;

            m_LineRenderer.startColor = m_ColorArray[Random.Range(MIN_COLOR_INDEX, MAX_COLOR_INDEX)];
            m_LineRenderer.endColor = m_ColorArray[Random.Range(MIN_COLOR_INDEX, MAX_COLOR_INDEX)];
            m_LineRenderer.SetPosition(0, transform.position);
             m_LineRenderer.SetPosition(1, laserEnd);

            if(hit.collider.tag == StringConstantUtility.PLAYER_TAG)
            {
                hit.collider.gameObject.GetComponent<PlayerHealth>().KillPlayer();
            }
        }
    }

    #endregion Private Methods
}
