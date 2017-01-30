﻿using UnityEngine;
using System.Collections;

public class SmartCameraYPosition : MonoBehaviour {

    [SerializeField]
    private float _adjustSpeed;

    private GameObject _player;
    private Vector3 _currPlayerPos;

    private bool _movingRight = false;
    public bool MovingRight {
        set { _movingRight = value; }
    }
    private bool _adjusting = false;

    private float _startTime = 0.0f;

    private void OnEnable() {
        _player = GameObject.FindGameObjectWithTag("Player");
        _startTime = Time.time;
    }

    private void LateUpdate() {

        // EACH FRAME, DETERMINE THE PLAYER'S POSITION ON THE SCREEN TO PROPERLY ADJUST THE CAMERA.
        _currPlayerPos = Camera.main.WorldToViewportPoint(_player.transform.position);


        // WHEN THE PLAYER IS MOVING TO THE RIGHT
        if (_movingRight) {
            // CHECK IF THE PLAYER IS CLOSE TO THE LEFT SIDE OF THE SCREEN
            if (_currPlayerPos.x <= 0.3f) {
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
