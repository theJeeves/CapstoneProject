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

    private Vector2 _direction;
    private LineRenderer _lr;
    private Color[] _colorArray = { Color.blue, Color.cyan, Color.green, Color.magenta, Color.red, Color.yellow };

    #endregion Private Fields


    #region Private Initializers
    // Use this for initialization
    void Start()
    {
        _lr = GetComponentInChildren<LineRenderer>();
    }

    #endregion Private Initializers

    #region Private Methods
    // Update is called once per frame
    private void Update()
    {
        _direction = endBarrel.transform.position - startBarrel.transform.position;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, _direction, Mathf.Infinity, _whatToHit);
        if (hit.collider != null)
        {
            Vector2 laserEnd = new Vector2(hit.point.x, hit.point.y);
            _lr.startWidth = LASER_START_WIDTH;
            _lr.endWidth = LASER_END_WIDTH;

            _lr.startColor = _colorArray[Random.Range(MIN_COLOR_INDEX, MAX_COLOR_INDEX)];
            _lr.endColor = _colorArray[Random.Range(MIN_COLOR_INDEX, MAX_COLOR_INDEX)];
            _lr.SetPosition(0, transform.position);
             _lr.SetPosition(1, laserEnd);

            if(hit.collider.tag == StringConstantUtility.PLAYER_TAG)
            {
                hit.collider.gameObject.GetComponent<PlayerHealth>().KillPlayer();
            }
        }
    }

    #endregion Private Methods
}
