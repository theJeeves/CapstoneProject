using UnityEngine;
using System.Collections;

public class SmartCameraXPosition : MonoBehaviour {

    #region Private Fields
    [SerializeField]
    private float _adjustSpeed;

    private GameObject _player;
    private Vector3 _currPlayerPos;
    private float _movementDelay = 0.5f;
    private float _movementTimer = 0.0f;
    private bool _adjusting = false;
    private float _startTime = 0.0f;

    #endregion Private Fields

    private void OnEnable() {

        PlayerHealth.UpdateHealth += UponDeath;

        _player = GameObject.FindGameObjectWithTag(StringConstantUtility.PLAYER_TAG);
        _startTime = Time.time;

        if (_player != null) {
            if (Camera.main.WorldToViewportPoint(_player.transform.position).x >= 0.5f) _movingRight = false;
            else if (Camera.main.WorldToViewportPoint(_player.transform.position).x < 0.5f) _movingRight = true;
        }
    }

    private void OnDisable() {
        PlayerHealth.UpdateHealth -= UponDeath;
    }

    #region Properties
    private bool _movingRight = false;
    public bool MovingRight {
        set { _movingRight = value; }
    }

    #endregion Properties

    #region Private Methods
    private void Update() {
        if (_player == null) {
            _player = GameObject.FindGameObjectWithTag(StringConstantUtility.PLAYER_TAG);

            if (Camera.main.WorldToViewportPoint(_player.transform.position).x > 0.5f) _movingRight = false;
            else if (Camera.main.WorldToViewportPoint(_player.transform.position).x < 0.5f) _movingRight = true;
        }
    }

    private void LateUpdate() {

        if (TimeTools.TimeExpired(ref _movementTimer)) {

            // EACH FRAME, DETERMINE THE PLAYER'S POSITION ON THE SCREEN TO PROPERLY ADJUST THE CAMERA.
            _currPlayerPos = Camera.main.WorldToViewportPoint(_player.transform.position);

            // WHEN THE PLAYER IS MOVING TO THE RIGHT
            if (_movingRight) {
                // CHECK IF THE PLAYER IS CLOSE TO THE LEFT SIDE OF THE SCREEN
                if (_currPlayerPos.x <= 0.35f) {
                    _movingRight = false;
                    _adjusting = true;
                    _startTime = 0.0f;
                }
                // IF THE CAMERA NEEDS TO ADJUST, KEEP ADJUSTING LEFT UNTIL THE PLAYER IS IN THE CENTER OF THE SCREEN.
                else if (_adjusting) {
                    if (_startTime == 0.0f) {
                        _startTime = Time.time;
                    }

                    transform.position = new Vector3(Mathf.SmoothStep(transform.position.x, _player.transform.position.x, (Time.time - _startTime) / _adjustSpeed),
                        transform.position.y, transform.position.z);

                    // ONCE THE PLAYER IS IN THE CENTER, STOP ADJUSTING AND RESET THE TIMER.
                    if (_currPlayerPos.x <= 0.5f) {
                        _adjusting = false;
                        _startTime = 0.0f;
                    }
                }

                //IF THE PLYAER REACHES THE HALF WAY POINT ON THE SCREEN, START FOLLOWING THE PLAYER
                else if (_currPlayerPos.x >= 0.5f) {
                    transform.position = new Vector3(_player.transform.position.x, transform.position.y, transform.position.z);
                }
            }

            // WHEN THE PLAYER IS MOVING TO THE LEFT
            else if (!_movingRight) {

                // CHECK IF THE PLAYER IS CLOSE TO THE RIGHT SIDE OF THE SCREEN
                if (_currPlayerPos.x >= 0.6f) {
                    _movingRight = true;
                    _adjusting = true;
                    _startTime = 0.0f;
                }
                // IF THE CAMERA NEEDS TO ADJUST, KEEP ADJUSTING RIGHT UNTIL THE PLAYER IS IN THE CENTER OF THE SCREEN.
                else if (_adjusting) {
                    if (_startTime == 0.0f) {
                        _startTime = Time.time;
                    }

                    transform.position = new Vector3(Mathf.SmoothStep(transform.position.x, _player.transform.position.x, (Time.time - _startTime) / _adjustSpeed),
                        transform.position.y, transform.position.z);

                    // ONCE THE PLAYER IS IN THE CENTER, STOP ADJUSTING AND RESET THE TIMER
                    if (_currPlayerPos.x >= 0.5f) {
                        _adjusting = false;
                        _startTime = 0.0f;
                    }
                }
                // KEEP ADJUSTING THE CAMERA TO THE right UNTIL THE PLAYER IS ALMOST IN THE CENTER OF THE SCREEN (X-AXIS WISE)
                else if (_currPlayerPos.x <= 0.5f) {
                    transform.position = new Vector3(_player.transform.position.x, transform.position.y, transform.position.z);
                }
            }
        }
    }

    private void UponDeath(object sender, int health) {

        if (health <= 0) {
            _movementTimer = _movementDelay;
        }
    }

    #endregion Private Methods
}
