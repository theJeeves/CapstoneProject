using UnityEngine;
using System.Collections;

public class LaserSights : MonoBehaviour {

    [SerializeField]
    private SOEffects _SOEffectHandler;
    [SerializeField]
    private LayerMask _whatToHit;
    [SerializeField]
    private Transform _endOfBarrel;

    private PolygonCollider2D _playerBox;
    private Vector3 _playerPos;
    private Vector2 _obstruction;
    private LineRenderer _renderer;
    private GameObject _laserEffect;

    private void Awake() {
        //_playerBox = GameObject.FindGameObjectWithTag("Player").GetComponent<PolygonCollider2D>();
        _renderer = GetComponent<LineRenderer>();
    }

    private void OnEnable() {
        _laserEffect = _SOEffectHandler.PlayEffect(EffectEnums.SniperLaserEffect, _endOfBarrel.position);
    }

    private void OnDisable() {
        _SOEffectHandler.StopEffect(_laserEffect);
    }

    private void Update() {

        if (_playerBox == null) {
            _playerBox = GameObject.FindGameObjectWithTag("Player").GetComponent<PolygonCollider2D>();
        }

        _playerPos = new Vector2(_playerBox.bounds.center.x, _playerBox.bounds.center.y + 7.5f);

        CheckCollisions();

        //Set the Start Position
        _renderer.SetPosition(0, _endOfBarrel.position);

        //Set the End Position
        if (_obstruction == Vector2.zero) {
            _renderer.SetPosition(1, _playerPos);
            _laserEffect.transform.position = _playerPos;
        }
        else {
            _renderer.SetPosition(1, _obstruction);
            _laserEffect.transform.position = _obstruction;
        }

        //Set the width of the line renderer
        _renderer.SetWidth(2.5f, 2.5f);
        _renderer.SetColors(Color.red, Color.red);
    }

    private void CheckCollisions() {

        // Send out a raycast to determine if the laser should be touching the player or a world object.
        RaycastHit2D hit = Physics2D.Raycast(_endOfBarrel.position, _playerPos - _endOfBarrel.position, 
            Vector3.Magnitude(_playerPos - _endOfBarrel.position), _whatToHit);

        if (hit.collider != null) {
            _obstruction = hit.collider.gameObject.tag == "Player" ? new Vector2(0.0f, 0.0f) : hit.point;
        }
    }
}
