using UnityEngine;
using System.Collections;

public class laserBeam : MonoBehaviour
{
    public delegate void LaserDealDamageEvent(int damage);

    private LineRenderer _lr;
    private Transform _laser;

    public Transform start;
    public Transform end;

    private RaycastHit2D _hit;

    [SerializeField]
    private LayerMask _whatToHit;

    [SerializeField]
    private float _laserTimer;

    [SerializeField]
    private int _damage;

    [SerializeField]
    private float _startDelay;

    private bool _delayFinished;
    private bool _laserActive = false;


    // Use this for initialization
    private void OnEnable()
    {
        _lr = GetComponentInChildren<LineRenderer>();
        _laser = transform.GetChild(0);
        drawLaser();
        StartCoroutine(Delay());
    }

    // Update is called once per frame
    //FIX THIS!!!!!!!!!!
    void Update()
    {
        if (_laser.gameObject.activeInHierarchy)
        {
            GetComponentInChildren<AudioSource>().Play();
            Debug.DrawLine(start.position, end.position, Color.red);
            _hit = Physics2D.Raycast(start.position, (end.position - start.position).normalized, (end.position - start.position).magnitude, _whatToHit);
            if (_hit.collider != null)
            {
                if (_hit.collider.tag == "Player")
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
    void drawLaser()
    {
        _lr.SetPosition(0, start.position);
        //_lr.SetPosition(1, start.position);
        _lr.SetWidth(5.0f, 5.0f);
        _lr.SetColors(Color.red, Color.red);
        _laser.gameObject.SetActive(false);
    }

    IEnumerator LaserOnOff()
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

    IEnumerator Delay()
    {
        _delayFinished = false;
        yield return new WaitForSeconds(_startDelay);
        _delayFinished = true;
    }
}