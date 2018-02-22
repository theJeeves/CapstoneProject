using UnityEngine;
using System.Collections;

public class laserBeam : MonoBehaviour
{
    #region Constants
    private const float LASER_START_WIDTH = 5.0f;
    private const float LASER_END_WIDTH = 5.0f;

    #endregion Constants

    #region Fields

    #region Public Fields
    public Transform start;
    public Transform end;

    #endregion Public Fields

    #region Private Fields
    [SerializeField]
    private LayerMask _whatToHit;
    [SerializeField]
    private float _laserTimer;
    [SerializeField]
    private int _damage;
    [SerializeField]
    private float _startDelay;
    [SerializeField]
    private bool _alwaysOn;

    private LineRenderer _lr;
    private Transform _laser;
    private RaycastHit2D _hit;
    private bool _delayFinished;
    private bool _laserActive = false;

    #endregion Private Fields

    #endregion Fields

    #region Private Initializers
    // Use this for initialization
    private void OnEnable()
    {
        _lr = GetComponentInChildren<LineRenderer>();
        _laser = transform.GetChild(0);
        drawLaser();
        StartCoroutine(Delay());
    }

    #endregion Private Initializers

    #region Private Methods
    private void Update()
    {
        if (_alwaysOn)
        {
            drawLaser();
            Debug.DrawLine(start.position, end.position, Color.red);
            _hit = Physics2D.Raycast(start.position, (end.position - start.position).normalized, (end.position - start.position).magnitude, _whatToHit);
            if (_hit.collider != null)
            {
                if (_hit.collider.tag == StringConstantUtility.PLAYER_TAG)
                {
                    _hit.collider.gameObject.GetComponent<PlayerHealth>().DecrementPlayerHealth(_damage, 0.0f, DamageEnum.LaserContinuous);

                }
            }
        }
        else
        {

            if (_laser.gameObject.activeInHierarchy)
            {
                GetComponentInChildren<AudioSource>().Play();
                Debug.DrawLine(start.position, end.position, Color.red);
                _hit = Physics2D.Raycast(start.position, (end.position - start.position).normalized, (end.position - start.position).magnitude, _whatToHit);
                if (_hit.collider != null)
                {
                    if (_hit.collider.tag == StringConstantUtility.PLAYER_TAG)
                    {
                        _hit.collider.gameObject.GetComponent<PlayerHealth>().KillPlayer();

                    }
                }
            }
            else if (!_laser.gameObject.activeInHierarchy && _delayFinished)
            {
                GetComponentInChildren<AudioSource>().Stop();
                if (!_laserActive)
                {
                    StartCoroutine(LaserOnOff());
                    _laserActive = true;
                }
            }
        }
    }
    private void drawLaser()
    {
        _lr.SetPosition(0, start.position);
        if (_alwaysOn)
        {
            _lr.SetPosition(1, end.position);
            _lr.startWidth = LASER_START_WIDTH;
            _lr.endWidth = LASER_END_WIDTH;
            _lr.startColor = Color.yellow;
            _lr.endColor = Color.yellow;
        }
        else {
            _lr.startWidth = LASER_START_WIDTH;
            _lr.endWidth = LASER_END_WIDTH;
            _lr.startColor = Color.red;
            _lr.endColor = Color.red;
            _laser.gameObject.SetActive(false);
        }
    }

    private IEnumerator LaserOnOff()
    {
        while (true)
        {
            yield return new WaitForSeconds(_laserTimer);
            _laser.gameObject.SetActive(true);
            _lr.SetPosition(1, end.position);

            yield return new WaitForSeconds(_laserTimer);
            _lr.SetPosition(1, start.position);
            _laser.gameObject.SetActive(false);
        }
    }

    private IEnumerator Delay()
    {
        _delayFinished = false;
        yield return new WaitForSeconds(_startDelay);
        _delayFinished = true;
    }

    #endregion Private Methods
}