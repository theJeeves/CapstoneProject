using UnityEngine;
using System.Collections;

public class LaserSights : MonoBehaviour {

    [SerializeField]
    private SOEffects _SOEffect;
    [SerializeField]
    private LayerMask _whatToHit;
    [SerializeField]
    private Transform _endOfBarrel;

    private Transform _player;
    private Vector2 _obstruction;
    private LineRenderer _renderer;
    private GameObject _laserEffect;

    private void Awake() {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _renderer = GetComponent<LineRenderer>();
    }

    private void OnEnable() {
        _laserEffect = _SOEffect.PlayVisualEffect(_endOfBarrel.position);
    }

    private void OnDisable() {
        _SOEffect.StopEffect(_laserEffect);
    }

    private void Update() {

        CheckCollisions();

        //Set the Start Position
        _renderer.SetPosition(0, _endOfBarrel.position);

        //Set the End Position
        if (_obstruction == Vector2.zero) {
            _renderer.SetPosition(1, _player.position);
            _laserEffect.transform.position = _player.position;
        }
        else {
            _renderer.SetPosition(1, _obstruction);
            _laserEffect.transform.position = _obstruction;
        }

        //Set the width of the line renderer
        _renderer.SetWidth(1.0f, 1.0f);
    }

    private void CheckCollisions() {

        // Send out a raycast to determine if the laser should be touching the player or a world object.
        RaycastHit2D hit = Physics2D.Raycast(_endOfBarrel.position, _player.position - _endOfBarrel.position, 
            Vector3.Magnitude(_player.position - _endOfBarrel.position), _whatToHit);

        if (hit.collider != null) {
            _obstruction = hit.collider.gameObject.tag == "Player" ? new Vector2(0.0f, 0.0f) : hit.point;
        }
    }
}
