using UnityEngine;
using System.Collections;

public class SmartCameraYPosition : MonoBehaviour {

    private GameObject _player;
    private Vector3 _currPlayerPos;

    private float _diff = 0.0f;

    private bool _startAdjustment = false;

    private void OnEnable() {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void LateUpdate() {

        // EACH FRAME, DETERMINE THE PLAYER'S POSITION ON THE SCREEN TO PROPERLY ADJUST THE CAMERA
        // AND ITS CURRENT Y VELOCITY
        _currPlayerPos = Camera.main.WorldToViewportPoint(_player.transform.position);

        if (_currPlayerPos.y >= 0.5f) {
            transform.position = new Vector3(transform.position.x, _player.transform.position.y, transform.position.z);
        }
        else if (_currPlayerPos.y <= 0.3f) {
            if (!_startAdjustment) {
                _startAdjustment = true;
                _diff = transform.position.y - _player.transform.position.y;
            }

            transform.position = new Vector3(transform.position.x, _diff + _player.transform.position.y, transform.position.z);
        }
        else {
            _startAdjustment = false;
        }
    }
}
