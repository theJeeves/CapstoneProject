using UnityEngine;
using System.Collections;

public class FocusCamera : MonoBehaviour {

    [SerializeField]
    private ScriptedCamera _scriptedCam;

    [SerializeField]
    private bool _leftAndRight;
    [SerializeField]
    private bool _upAndDown;

    [SerializeField]
    private Transform[] _triggerPoints;
    [SerializeField]
    private Vector3[] _cameraPositions;

    private Vector2 playerPos = Vector2.zero;
    private int length = 0;

    private void OnTriggerStay2D(Collider2D otherGO) {

        if (otherGO.tag == "Player") {

            playerPos = otherGO.transform.position;
            length = _triggerPoints.Length;

            if (length > 1) {
                for (int i = 0; i < length - 1; ++i) {

                    if (_leftAndRight) {
                        if (playerPos.x > _triggerPoints[i].position.x && playerPos.x < _triggerPoints[i + 1].position.x) {
                            _scriptedCam.MoveCamera(_cameraPositions[i]);
                        }
                    }
                    if (_upAndDown) {
                        if (playerPos.y > _triggerPoints[i].position.y && playerPos.y < _triggerPoints[i + 1].position.y) {
                            _scriptedCam.MoveCamera(_cameraPositions[i]);
                        }
                    }
                }
            }
            else if (length == 0) {
                _scriptedCam.MoveCamera(_cameraPositions[0]);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D otherGO) {

        if (gameObject.GetComponent<FocusCamera>().enabled) {

            if (otherGO.tag == "Player") {
                _scriptedCam.Reset();
            }
        }
    }
}
