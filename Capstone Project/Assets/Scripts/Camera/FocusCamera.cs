using UnityEngine;
using System.Collections;

public class FocusCamera : MonoBehaviour {

    [SerializeField]
    private ScriptedCamera _scriptedCam;
    [SerializeField]
    private bool _rightTrigger = false;

    private Vector3 _boxPos;

    private void OnEnable() {
        _boxPos.x = _rightTrigger ? transform.position.x + GetComponent<BoxCollider2D>().size.x / 2 :
            transform.position.x - GetComponent<BoxCollider2D>().size.x / 2;
    }

    private void OnTriggerEnter2D(Collider2D player) {

        if (player.tag == "Player") {

            _scriptedCam.IsRightTrigger = _rightTrigger;

            switch (_rightTrigger) {
                case true:

                    if (player.transform.position.x >= _boxPos.x) {
                        _scriptedCam.DisableScripts();
                        StartCoroutine(MoveCamera());
                    }
                    break;

                case false:

                    if (player.transform.position.x <= _boxPos.x) {
                        _scriptedCam.DisableScripts();
                        StartCoroutine(MoveCamera());
                    }
                    break;
            }
        }
    }

    private IEnumerator MoveCamera() {

        while (!_scriptedCam.MoveCamera(_boxPos)) {
            yield return 0;
        }
    }

    private void OnTriggerExit2D(Collider2D player) {

        if (player.tag == "Player") {

            _scriptedCam.IsRightTrigger = _rightTrigger;

            switch (_rightTrigger) {
                case true:
                    if (player.transform.position.x > _boxPos.x) {
                        _scriptedCam.EnableScripts();
                        StopAllCoroutines();
                    }
                    break;

                case false:
                    if (player.transform.position.x < _boxPos.x) {
                        _scriptedCam.EnableScripts();
                        StopAllCoroutines();
                    }
                    break;
            }
        }
    }
}
