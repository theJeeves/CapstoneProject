using UnityEngine;
using System.Collections;

/// <summary>
/// NOTE: WHEN USING WORLDTOVIEWPORTPOINT, BOTTOM LEFT IS (0, 0) AND TOP RIGHT IS (1, 1)
/// </summary>

public class SmartCameraYPosition : MonoBehaviour {

    [SerializeField]
    private float _adjustSpeed;

    private GameObject _player;
    private Vector3 _currPlayerPos;

    private bool _movingUp = false;
    public bool MovingUp {
        set { _movingUp = value; }
    }
    private bool _adjusting = false;

    private float _startTime = 0.0f;

    private void OnEnable() {
        _player = GameObject.FindGameObjectWithTag("Player");
        _startTime = Time.time;

        _movingUp = Camera.main.WorldToViewportPoint(_player.transform.position).y <= 0.3f ? false : true;
    }

    private void LateUpdate() {

        // EACH FRAME, DETERMINE THE PLAYER'S POSITION ON THE SCREEN TO PROPERLY ADJUST THE CAMERA.
        _currPlayerPos = Camera.main.WorldToViewportPoint(_player.transform.position);

        //Debug.Log("Moving Up: " + _movingUp + " " + _currPlayerPos.y);

        // WHEN THE PLAYER IS MOVING UP
        if (_movingUp) {

            if (_currPlayerPos.y > 0.7f) {
                _adjusting = true;
            }

            // CHECK IF THE PLAYER IS CLOSE BOTTOM OF THE SCREEN
            if (_currPlayerPos.y <= 0.3f) {
                _movingUp = false;
                _adjusting = true;
                _startTime = 0.0f;
            }
            // IF THE CAMERA NEEDS TO ADJUST, KEEP ADJUSTING LEFT UNTIL THE PLAYER IS IN THE CENTER OF THE SCREEN.
            else if (_adjusting) {
                if (_startTime == 0.0f) {
                    _startTime = Time.time;
                }

                transform.position = new Vector3(transform.position.x,
                    Mathf.SmoothStep(transform.position.y, _player.transform.position.y + 100.0f, (Time.time - _startTime) / _adjustSpeed), transform.position.z);

                // ONCE THE PLAYER AT THIS Y POSITION, STOP ADJUSTING AND RESET THE TIMER.
                if (_currPlayerPos.y <= 0.3f) {
                    _adjusting = false;
                    _startTime = 0.0f;
                }
            }
        }

        // WHEN THE PLAYER IS MOVING DOWN
        else if (!_movingUp) {

            if (_currPlayerPos.y < 0.3f) {
                _adjusting = true;
            }

            // CHECK IF THE PLAYER IS CLOSE TO THE TOP OF THE SCREEN
            if (_currPlayerPos.y >= 0.7f) {
                _movingUp = true;
                _adjusting = true;
                _startTime = 0.0f;
            }
            // IF THE CAMERA NEEDS TO ADJUST, KEEP ADJUSTING RIGHT UNTIL THE PLAYER IS IN THE CENTER OF THE SCREEN.
            else if (_adjusting) {
                if (_startTime == 0.0f) {
                    _startTime = Time.time;
                }

                transform.position = new Vector3(transform.position.x,
                    Mathf.SmoothStep(transform.position.y, _player.transform.position.y - 100.0f, (Time.time - _startTime) / _adjustSpeed), transform.position.z);

                // ONCE THE PLAYER IS AT THIS Y POSITION, STOP ADJUSTING AND RESET THE TIMER
                if (_currPlayerPos.y >= 0.25f) {
                    _adjusting = false;
                    _startTime = 0.0f;
                }
            }
        }
    }

    //[SerializeField]
    //private float _adjustSpeed;

    //private GameObject _player;
    //private Vector3 _currPlayerPos;

    //private float _diff = 0.0f;

    //private bool _startAdjustment = false;

    //private void OnEnable() {
    //    _player = GameObject.FindGameObjectWithTag("Player");
    //}

    //private void LateUpdate() {

    //    // EACH FRAME, DETERMINE THE PLAYER'S POSITION ON THE SCREEN TO PROPERLY ADJUST THE CAMERA
    //    // AND ITS CURRENT Y VELOCITY
    //    _currPlayerPos = Camera.main.WorldToViewportPoint(_player.transform.position);

    //    if (_currPlayerPos.y >= 0.5f) {
    //        transform.position = new Vector3(transform.position.x, _player.transform.position.y, transform.position.z);
    //    }
    //    else if (_currPlayerPos.y <= 0.3f) {
    //        if (!_startAdjustment) {
    //            _startAdjustment = true;
    //            _diff = transform.position.y - _player.transform.position.y;
    //        }

    //        transform.position = new Vector3(transform.position.x, _diff + _player.transform.position.y, transform.position.z);
    //    }
    //    else {
    //        _startAdjustment = false;
    //    }
    //}
}
