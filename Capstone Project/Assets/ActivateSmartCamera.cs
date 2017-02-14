using UnityEngine;
using System.Collections;

public class ActivateSmartCamera : MonoBehaviour {

    protected GameObject _camera;       // Reference to the main Camera

    private void OnEnable() {
        _camera = GameObject.FindGameObjectWithTag("SmartCamera");
    }

    private void OnTriggerEnter2D(Collider2D otherGO) {

        if (otherGO.gameObject.tag == "Player") {
            _camera.GetComponent<SmartCameraFOV>().enabled = true;
            _camera.GetComponent<SmartCameraXPosition>().enabled = true;
            _camera.GetComponent<SmartCameraYPosition>().enabled = true;
        }
    }

}
