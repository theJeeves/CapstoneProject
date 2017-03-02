

using UnityEngine;
using System.Collections;

public class rotatingLaser : MonoBehaviour
{
    private Vector2 _direction;
    private LineRenderer _lr;

    [SerializeField]
    private GameObject endBarrel;

    [SerializeField]
    private GameObject startBarrel;

    [SerializeField]
    private int _damage;

    [SerializeField]
    private LayerMask _whatToHit;


    // Use this for initialization
    void Start()
    {
        _lr = GetComponentInChildren<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        _direction = endBarrel.transform.position - startBarrel.transform.position;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, _direction, Mathf.Infinity, _whatToHit);
        if (hit.collider != null)
        {
                Vector2 laserEnd = new Vector2(hit.point.x, hit.point.y);
            //float _height = Mathf.Abs(hit.point.y - transform.position.y);
            //float _width = Mathf.Abs(hit.point.x - transform.position.x);
            //float _distance = Mathf.Sqrt((_height * _height) + (_width * _width));
            _lr.SetWidth(2.5f, 2.5f);


            _lr.SetColors(Color.red, Color.red);
            _lr.SetPosition(0, transform.position);
             _lr.SetPosition(1, laserEnd);
            if(hit.collider.tag == "Player")
            {
                GameObject otherGO = GameObject.FindGameObjectWithTag("Player");
                otherGO.GetComponent<PlayerHealth>().DecrementPlayerHealth(_damage);
            }


        }

    }
}
