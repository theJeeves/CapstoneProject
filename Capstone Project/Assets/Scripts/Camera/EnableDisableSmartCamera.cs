using UnityEngine;
using System.Collections;

public class EnableDisableSmartCamera : MonoBehaviour {

    [Header("Smart Camera X")]
    [SerializeField]
    private bool _XEnableOnEnter = false;
    [SerializeField]
    private bool _XDisableOnEnter = false;
    [SerializeField]
    private bool _XEnableOnExit = false;
    [SerializeField]
    private bool _XDisableOnExit = false;

    [Space]
    [Header("Smart Camera Y")]
    [SerializeField]
    private bool _YEnableOnEnter = false;
    [SerializeField]
    private bool _YDisableOnEnter = false;
    [SerializeField]
    private bool _YEnableOnExit = false;
    [SerializeField]
    private bool _YDisableOnExit = false;

    //[Space]
    //[Header("Field Of View")]
    //[SerializeField]
    //private bool _FOVDisableOnEnter = false;
    //[SerializeField]
    //private bool _FOVEnableOnExit = false;

    private GameObject _camera;       // Reference to the main Camera

    private void OnEnable() {
        _camera = GameObject.FindGameObjectWithTag("SmartCamera");
    }

    private void OnTriggerEnter2D(Collider2D otherGO) {

        if (otherGO.gameObject.tag == "Player") {
            if (_XDisableOnEnter) {
                //_camera.GetComponent<SmartCameraFOV>().enabled = false;
                _camera.GetComponent<SmartCameraXPosition>().enabled = false;
            }
            else if (_XEnableOnEnter) {
                _camera.GetComponent<SmartCameraXPosition>().enabled = true;
            }

            if (_YDisableOnEnter) {
                _camera.GetComponent<SmartCameraYPosition>().enabled = false;
            }
            else if (_YEnableOnEnter) {
                _camera.GetComponent<SmartCameraYPosition>().enabled = true;
            }

            //if (_FOVDisableOnEnter) {
            //    _camera.GetComponent<SmartCameraFOV>().enabled = false;
            //}
        }
    }

    private void OnTriggerExit2D(Collider2D otherGO) {

        if (otherGO.gameObject.tag == "Player") {
            if (_XEnableOnExit) {
                //_camera.GetComponent<SmartCameraFOV>().enabled = true;
                _camera.GetComponent<SmartCameraXPosition>().enabled = true;
            }
            else if (_XDisableOnExit) {
                _camera.GetComponent<SmartCameraXPosition>().enabled = false;
            }

            if (_YEnableOnExit) {
                _camera.GetComponent<SmartCameraYPosition>().enabled = true;
            }
            else if (_YDisableOnExit) {
                _camera.GetComponent<SmartCameraYPosition>().enabled = false;
            }

            //if (_FOVEnableOnExit) {
            //    _camera.GetComponent<SmartCameraFOV>().enabled = true;
            //}
        }
    }
}
