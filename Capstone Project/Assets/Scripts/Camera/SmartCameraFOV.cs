using UnityEngine;
using System.Collections;

public class SmartCameraFOV : MonoBehaviour {

    [SerializeField]
    private float _minCamSize;
    [SerializeField]
    private float _maxCamSize;
    [SerializeField]
    private float _adjustSpeed;


    private float _currCamSize;
    private float _curreVelocity;

    private GameObject _player;
    private float _timer;

    private float _startTime;

    private bool _decraseing;
    private bool _increasing;

    private void OnEnable() {
        _player = GameObject.FindGameObjectWithTag("Player");
    }
	
	// Update is called once per frame
	private void LateUpdate () {

        _currCamSize = Camera.main.orthographicSize;
        _curreVelocity = Mathf.Abs(_player.GetComponent<Rigidbody2D>().velocity.x);

        // DECREASE THE SIZE OF TEH CAMERA IF THE PLAYER SLOWS DOWN
        if (_currCamSize > _minCamSize && _curreVelocity < 175.0f) {
            _timer = _timer > 0.0f ? _timer : Time.time;
            // WAIT 1.5 SECONDS BEFORE THE CAMERA SHRINKS BACK DOWN
            if (Time.time - _timer >= 1.5f) {
                if (!_decraseing) {
                    _increasing = false;
                    _decraseing = true;
                    _startTime = Time.time;
                }
                Camera.main.orthographicSize = Mathf.SmoothStep(_currCamSize, _minCamSize - 0.1f, (Time.time - _startTime) / _adjustSpeed);
            }
        }
        // INCREASE THE SIZE OF THE CAMERA IF THE PLAYER SPEEDS UP
        else if (_currCamSize < _maxCamSize && _curreVelocity >= 175) {
            if (!_increasing) {
                _decraseing = false;
                _increasing = true;
                _startTime = Time.time;
            }
            Camera.main.orthographicSize = Mathf.SmoothStep(_currCamSize, _maxCamSize + 0.1f, (Time.time - _startTime) / _adjustSpeed);
            _timer = 0.0f;
        }
        else {
            _timer = 0.0f;
            _increasing = false;
            _decraseing = false;
        }
    }
}
