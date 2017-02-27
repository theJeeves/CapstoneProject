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
    private bool _startYAdjustment = false;

    private float _startTime = 0.0f;
    private float _diff = 0.0f;
    private bool _timeSet = false;

    private float _movementTimer = 0.0f;
    private float _movementDelay = 0.5f;

    private void OnEnable() {
        PlayerHealth.UpdateHealth += UponDeath;

        _player = GameObject.FindGameObjectWithTag("Player");
        _startTime = Time.time;

        _movingUp = Camera.main.WorldToViewportPoint(_player.transform.position).y <= 0.3f ? false : true;
    }

    private void OnDisable() {
        PlayerHealth.UpdateHealth -= UponDeath;
    }

    private void LateUpdate() {

        if (Time.time - _movementTimer > _movementDelay) {
            // EACH FRAME, DETERMINE THE PLAYER'S POSITION ON THE SCREEN TO PROPERLY ADJUST THE CAMERA.
            _currPlayerPos = Camera.main.WorldToViewportPoint(_player.transform.position);


            // WHEN THE PLAYER IS MOVING UP
            if (_movingUp) {

                if (_currPlayerPos.y > 0.6f) {
                    _adjusting = true;
                }

                // CHECK IF THE PLAYER IS CLOSE BOTTOM OF THE SCREEN
                if (_currPlayerPos.y < 0.35f) {
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
                    if (_currPlayerPos.y <= 0.31f) {
                        _adjusting = false;
                        _startTime = 0.0f;
                    }
                }
            }

            // WHEN THE PLAYER IS MOVING DOWN
            else if (!_movingUp) {

                //if (_currPlayerPos.y < 0.3f) {
                //    _adjusting = true;
                //}

                // CHECK IF THE PLAYER IS CLOSE TO THE TOP OF THE SCREEN
                if (_currPlayerPos.y >= 0.45f) {
                    _movingUp = true;
                    _adjusting = true;
                    _startTime = 0.0f;
                }
                // IF THE CAMERA NEEDS TO ADJUST, KEEP ADJUSTING RIGHT UNTIL THE PLAYER IS IN THE CENTER OF THE SCREEN.
                //else if (_adjusting) {
                //    if (_startTime == 0.0f) {
                //        //_startTime = Time.time;
                //    }

                    //transform.position = new Vector3(transform.position.x,
                    //        Mathf.SmoothStep(transform.position.y, _player.transform.position.y - 250.0f, (Time.time - _startTime) / _adjustSpeed), transform.position.z);

                    // ONCE THE PLAYER IS AT THIS Y POSITION, STOP ADJUSTING AND RESET THE TIMER
                    //if (_currPlayerPos.y >= 0.3f) {
                    //    _adjusting = false;
                    //    //_startTime = 0.0f;
                    //    //_DownwardCompComplete = true;
                    //}
                //}

                if (_currPlayerPos.y < 0.3f) {
                    if (!_timeSet) {
                        _startTime = Time.time;
                        _timeSet = true;
                    }
                    transform.position = new Vector3(transform.position.x,
                            Mathf.SmoothStep(transform.position.y, _player.transform.position.y - 150.0f, (Time.time - _startTime) / _adjustSpeed), transform.position.z);
                }
                else {
                    _timeSet = false;
                }

                if (_player.GetComponent<Rigidbody2D>().velocity.y <= -200.0f) {

                    Debug.Log(_currPlayerPos.y);

                    if (!_startYAdjustment) {
                        _startYAdjustment = true;
                        _diff = transform.position.y - _player.transform.position.y;
                    }

                    transform.position = new Vector3(transform.position.x, _diff + _player.transform.position.y, transform.position.z);
                    _timeSet = false;
                }
                else {
                    _startYAdjustment = false;
                }
            }
        }
    }

    private void UponDeath(int health) {

        if (health <= 0) {
            _movementTimer = Time.time;
        }
    }
}