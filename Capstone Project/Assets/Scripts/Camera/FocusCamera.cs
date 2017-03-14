using UnityEngine;
using System.Collections;

public class FocusCamera : MonoBehaviour {

    [SerializeField]
    private ScriptedCamera _scriptedCam;

    //[SerializeField]
    private bool _leftAndRight = true;
    //[SerializeField]
    private bool _upAndDown;

    [SerializeField]
    private float _adjustDuration = 3.0f;

    [SerializeField]
    private Transform[] _triggerPoints;
    [SerializeField]
    private Vector3[] _cameraPositions;
    [SerializeField]
    private bool[] _linearMovement;

    private Vector2 playerPos = Vector2.zero;
    private int length = 0;
    private float _movementTimer = 0.0f;
    private float _movementDelay = 0.5f;

    private Vector3 _camOrigin = Vector3.zero;
    private bool _originSet = false;

    private BoxCollider2D _collider;

    private void OnEnable() {
        PlayerHealth.UpdateHealth += UponDeath;
        _collider = GetComponent<BoxCollider2D>();
    }

    private void OnDisable() {
        PlayerHealth.UpdateHealth -= UponDeath;
    }

    private void OnTriggerEnter2D(Collider2D otherGO) {
        if (_collider.enabled && otherGO.tag == "Player") {
            _scriptedCam.SetAdjustSpeed(_adjustDuration);
        }
    }

    private void OnTriggerStay2D(Collider2D otherGO) {

        if (_collider.enabled && otherGO.tag == "Player" &&  Time.time - _movementTimer > _movementDelay) {

            playerPos = otherGO.transform.position;
            length = _triggerPoints.Length;

            if (length > 1) {
                for (int i = 0; i < length - 1; ++i) {

                    if (_leftAndRight) {
                        if (playerPos.x > _triggerPoints[i].position.x && playerPos.x < _triggerPoints[i + 1].position.x) {

                            if (_linearMovement.Length > 0 && _linearMovement[i]) {

                                float difference = _triggerPoints[i + 1].position.x - _triggerPoints[i].position.x;
                                float percentage = 1.0f - ((_triggerPoints[i + 1].position.x - playerPos.x) / difference);
                                SetCameraOrigin();
                                _scriptedCam.LinearlyMoveCamera(percentage, _camOrigin, _cameraPositions[i]);
                            }
                            else {
                                _scriptedCam.MoveCamera(_cameraPositions[i]);
                            }
                        }
                    }
                    //if (_upAndDown) {
                    //    if (playerPos.y > _triggerPoints[i].position.y && playerPos.y < _triggerPoints[i + 1].position.y) {
                    //        _scriptedCam.MoveCamera(_cameraPositions[i]);
                    //    }
                    //}
                }
            }
            else if (length == 0) {
                _scriptedCam.MoveCamera(_cameraPositions[0]);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D otherGO) {

        if (_collider.enabled && gameObject.GetComponent<FocusCamera>().enabled) {

            if (otherGO.tag == "Player") {
                _scriptedCam.Reset();
                //_originSet = false;
            }
        }
    }

    private void UponDeath(int health) {
        if (health <= 0) {
            _movementTimer = Time.time;
        }
    }

    private void SetCameraOrigin() {

        if (!_originSet) {
            _camOrigin = GameObject.FindGameObjectWithTag("SmartCamera").transform.position;
            _originSet = true;
        }
    }
}
