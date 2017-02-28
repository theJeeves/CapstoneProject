using UnityEngine;
using System.Collections;

public class SmartCameraFOV : MonoBehaviour {

    //[SerializeField]
    //private float _minCamSize;
    //[SerializeField]
    //private float _maxCamSize;
    [SerializeField]
    private float _adjustSpeed;
    [SerializeField]
    private float _targetZ = -100.0f;
    private bool _adjust = true;

    private Vector3 _currPosition = Vector3.zero;
    private float _curreVelocity;

    private GameObject _player;
    private float _timer;

    private float _startTime;

    private bool _decraseing;
    private bool _increasing;

    private void OnEnable() {
        _player = GameObject.FindGameObjectWithTag("Player");
        _startTime = Time.time;
    }
	
	// Update is called once per frame
	private void LateUpdate () {

        if (_adjust) {

            _currPosition = transform.position;

            if (_targetZ >= _currPosition.z - 1.0f || _targetZ <= _currPosition.z + 1.0f) {
                _adjust = false;
            }
        else {
                transform.position = new Vector3(_currPosition.x, _currPosition.y,
                    Mathf.SmoothStep(_currPosition.z, _targetZ - 0.1f, (Time.time - _startTime) / _adjustSpeed));
            }
        }

        //_currCamSize = Camera.main.orthographicSize;
        //_curreVelocity = Mathf.Abs(_player.GetComponent<Rigidbody2D>().velocity.x);

        //// DECREASE THE SIZE OF TEH CAMERA IF THE PLAYER SLOWS DOWN
        //if (_currCamSize > _minCamSize && _curreVelocity < 175.0f) {
        //    _timer = _timer > 0.0f ? _timer : Time.time;
        //    // WAIT 1.5 SECONDS BEFORE THE CAMERA SHRINKS BACK DOWN
        //    if (Time.time - _timer >= 1.5f) {
        //        if (!_decraseing) {
        //            _increasing = false;
        //            _decraseing = true;
        //            _startTime = Time.time;
        //        }
        //        Camera.main.orthographicSize = Mathf.SmoothStep(_currCamSize, _minCamSize - 0.1f, (Time.time - _startTime) / _adjustSpeed);
        //    }
        //}
        //// INCREASE THE SIZE OF THE CAMERA IF THE PLAYER SPEEDS UP
        //else if (_currCamSize < _maxCamSize && _curreVelocity >= 175) {
        //    if (!_increasing) {
        //        _decraseing = false;
        //        _increasing = true;
        //        _startTime = Time.time;
        //    }
        //    Camera.main.orthographicSize = Mathf.SmoothStep(_currCamSize, _maxCamSize + 0.1f, (Time.time - _startTime) / _adjustSpeed);
        //    _timer = 0.0f;
        //}
        //else {
        //    _timer = 0.0f;
        //    _increasing = false;
        //    _decraseing = false;
        //}
    }

    public void SetZCamera(float target) {
        _adjust = true;
        _targetZ = target;
        _timer = Time.time;
    }
}
