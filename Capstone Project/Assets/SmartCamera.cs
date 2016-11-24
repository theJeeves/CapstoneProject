using UnityEngine;
using System.Collections;

public class SmartCamera : MonoBehaviour {

    [SerializeField]
    private float _minCamSize;
    [SerializeField]
    private float _maxCamSize;
    [SerializeField]
    private float _adjustSpeed;


    private float _currCamSize;
    private float _curreVelocity;
    private Vector3 _currPlayerPos;

    private GameObject _player;
    private bool _movingRight = true;
    private float _timer;

    private float _startTime;

    private void OnEnable() {
        _player = GameObject.FindGameObjectWithTag("Player");

        //StartCoroutine(AdjustForXVelocity());
    }
	
	// Update is called once per frame
	void Update () {

        _currCamSize = Camera.main.orthographicSize;
        _curreVelocity = Mathf.Abs(_player.GetComponent<Rigidbody2D>().velocity.x);
        _currPlayerPos = Camera.main.WorldToViewportPoint(_player.transform.position);

        if (_currCamSize > _minCamSize && _curreVelocity < 175) { 
            _timer = _timer > 0.0f ? _timer : Time.time;
            if (Time.time - _timer >= 1.5f) {
                if (Time.time - _timer <= 1.51f) {
                    _startTime = Time.time;
                }
                Camera.main.orthographicSize = Mathf.SmoothStep(_currCamSize, _minCamSize - 0.1f, (Time.time - _startTime) / _adjustSpeed);
            }
        }
        // INCREASE THE SIZE OF THE CAMERA IF THE PLAYER SPEEDS UP
        else if (_currCamSize < _maxCamSize && _curreVelocity >= 175) {
            if (_startTime == 0.0f) {
                _startTime = Time.time;
            }
            Camera.main.orthographicSize = Mathf.SmoothStep(_currCamSize, _maxCamSize + 0.1f, (Time.time - _startTime) / _adjustSpeed);
            _timer = 0.0f;
        }
        else {
            _startTime = 0.0f;
        }

        if (_movingRight) {
            // iF THE PLAYER IS MOVING TO THE RIGHT
            //IF THE PLYAER REACHES THE HALF WAY POINT ON THE SCREEN, START FOLLOWING THE PLAYER
            if (_currPlayerPos.x >= 0.5f) {
                transform.position = new Vector3(_player.transform.position.x, transform.position.y, transform.position.z);
            }

            //IF THE PLAYER STARTS WALKING TO THE LEFT, THE CAMERA WILL WAIT UNTIL THEY ARE CLOSE
            // TO THE LEFT SIDE OF THE SCREEN BEFORE MOVING THEM BACK TO THE NEAR CENTER OF THE SCREEN.
            else if (_currPlayerPos.x <= 0.2f) {
                StartCoroutine(AdjustForXPosition());
            }
        }

        // EVERYTHING IS THE SAME AS ABOVE, BUT THIS IS FOR WHEN THE PLAYER IS MOVING TO THE LEFT
        else if (!_movingRight) {
            if (_currPlayerPos.x <= 0.5f) {
                transform.position = new Vector3(_player.transform.position.x, transform.position.y, transform.position.z);
            }
            else if (_currPlayerPos.x >= 0.7) {
                StartCoroutine(AdjustForXPosition());
            }
        }

        // Y-POSITION CHECKS
        if (_currPlayerPos.y >= 0.5f) {
            transform.position = new Vector3(transform.position.x, _player.transform.position.y, transform.position.z);
        }
        else if (_currPlayerPos.y <= 0.25f) {
            StartCoroutine(AdjustForYPosition());
        }
    }

    private IEnumerator AdjustForXPosition() {

        if (_movingRight) {
            // KEEP ADJUSTING THE CAMERA TO THE LEFT UNTIL THE PLAYER IS ALMOST IN THE CENTER OF THE SCREEN (X-AXIS WISE)
            while (Camera.main.WorldToViewportPoint(_player.transform.position).x <= 0.5f) {
                transform.position += Vector3.left * _adjustSpeed;
                yield return 0;
            }
            _movingRight = false;
        }
        else if (!_movingRight) {
            // KEEP ADJUSTING THE CAMERA TO THE right UNTIL THE PLAYER IS ALMOST IN THE CENTER OF THE SCREEN (X-AXIS WISE)
            while (Camera.main.WorldToViewportPoint(_player.transform.position).x >= 0.5f) {
                transform.position += Vector3.right * _adjustSpeed;
                yield return 0;
            }
            _movingRight = true;
        }
    }

    private IEnumerator AdjustForYPosition() {

        //KEEP ADJUSTING THE CAMERA DOWN UNTIL THE PLAYER IS ALMOST IN THE CENTER OF THE SCREEN (Y-AXIS WISE)
        while (Camera.main.WorldToViewportPoint(_player.transform.position).y < 0.5f) {
            transform.position += Vector3.down * _adjustSpeed;
            yield return 0;
        }
        while(_player.GetComponent<Rigidbody2D>().velocity.y < 0) {
            transform.position = new Vector3(transform.position.x, _player.transform.position.y, transform.position.z);
            yield return 0;
        }

        while (Camera.main.WorldToViewportPoint(_player.transform.position).y < 0.35f) {
            transform.position += Vector3.down * _adjustSpeed;
            yield return 0;
        }

    }

    private IEnumerator AdjustForXVelocity() {

        // ALWAYS BE CHECKING FOR THESE CONDITIONS
        while (true) {
            // GO BACK TO THE DEFAULT CAMERA SIZE IF THE PLAYER SLOWS DOWN
            if (Mathf.Abs(_player.GetComponent<Rigidbody2D>().velocity.x) < 175 &&
                Camera.main.orthographicSize > _minCamSize) {
                _timer = _timer > 0.0f ? _timer : Time.time;
                if (Time.time - _timer >= 1.5f) {
                    Camera.main.orthographicSize -= 1.0f;
                }
            }
            // INCREASE THE SIZE OF THE CAMERA IF THE PLAYER SPEEDS UP
            else if (Mathf.Abs(_player.GetComponent<Rigidbody2D>().velocity.x) >= 175 &&
                Camera.main.orthographicSize <= _maxCamSize) {
                Camera.main.orthographicSize += 1.0f;
                _timer = 0;
            }

            yield return 0;
        }
    }
}
