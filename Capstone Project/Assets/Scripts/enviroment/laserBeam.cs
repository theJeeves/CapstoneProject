using UnityEngine;
using System.Collections;

public class laserBeam : MonoBehaviour
{
    public delegate void LaserDealDamageEvent(int damage);
    public static event LaserDealDamageEvent DecrementPlayerHealth;


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



    // Use this for initialization
    void Start()
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
            Debug.DrawLine(start.position, end.position, Color.red);
            _hit = Physics2D.Raycast(start.position, (end.position - start.position).normalized, (end.position - start.position).magnitude, _whatToHit);
            if (_hit.collider != null)
            {
                if (_hit.collider.tag == "Player")
                {
                        GameObject otherGO = GameObject.FindGameObjectWithTag("Player");
                        otherGO.GetComponent<PlayerHealth>().DecrementPlayerHealth(_damage);
                    
                }
            }
        }
        else if (!_laser.gameObject.activeInHierarchy && _delayFinished)
        {
            StartCoroutine(LaserOnOff());
        }

        
    }
    void drawLaser()
    {
        _lr.SetPosition(0, start.position);
        //_lr.SetPosition(1, start.position);
        _lr.SetWidth(5.0f, 5.0f);
        _laser.gameObject.SetActive(false);
    }

    IEnumerator LaserOnOff()
    {
        
        yield return new WaitForSeconds(_laserTimer);
        _laser.gameObject.SetActive(true);
        _lr.SetPosition(1, end.position);

        yield return new WaitForSeconds(_laserTimer);
        _lr.SetPosition(1, start.position);
        _laser.gameObject.SetActive(false);

    }

    IEnumerator Delay()
    {
        _delayFinished = false;
        yield return new WaitForSeconds(_startDelay);
        _delayFinished = true;
        StartCoroutine(LaserOnOff());
    }
}