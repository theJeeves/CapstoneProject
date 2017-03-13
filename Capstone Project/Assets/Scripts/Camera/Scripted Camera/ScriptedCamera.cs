using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName ="Scripted Camera/New Scripted Camera Handler")]
public class ScriptedCamera : ScriptableObject {

    //The speed at which the FOV will adjust
    [SerializeField]
    protected float _adjustSpeed = 10.0f;

    //private bool largerFOV = false;

    protected GameObject _camera;       // Reference to the main Camera
    protected float _time = 0.0f;       // Used for timing and delay purposes

    private Vector3 _initialTarget;

    private void OnEnable() {
        // Get a reference to the main camera to be used with the rest of the script.
        _camera = GameObject.FindGameObjectWithTag("SmartCamera");
        _initialTarget = Vector3.zero;
    }

    public void MoveCamera(Vector3 target) {

        if (_initialTarget != target) {
            _time = Time.time;
            _initialTarget = target;
        }

        if (_camera == null) {
            // Get a reference to the main camera to be used with the rest of the script.
            _camera = GameObject.FindGameObjectWithTag("SmartCamera");
        }

        if (_camera.transform.position != target) { 
            _camera.transform.position = new Vector3(Mathf.SmoothStep(_camera.transform.position.x, target.x, (Time.time - _time) / _adjustSpeed),
                Mathf.SmoothStep(_camera.transform.position.y, target.y, (Time.time - _time) / _adjustSpeed),
                Mathf.SmoothStep(_camera.transform.position.z, target.z, (Time.time - _time) / _adjustSpeed));
        }
    }

    public void LinearlyMoveCamera(float percentage, Vector3 originPos, Vector3 target) {
        //if (_camera.transform.position != target) {
            _camera.transform.position = new Vector3(percentage * (target.x - originPos.x) + originPos.x, 
                                                     percentage * (target.y - originPos.y) + originPos.y, 
                                                     percentage * (target.z - originPos.z) + originPos.z);
        //}
    }

    //Depending on what the developer wants to do, EnableScripts and DisableScripts will turn on/off
    //when a scripted camera event is needed. 
    public void EnableScripts() {
        //_camera.GetComponent<SmartCameraFOV>().enabled = true;
        _camera.GetComponent<SmartCameraXPosition>().enabled = true;
        _time = 0.0f;
    }

    public void DisableScripts() {
        //_camera.GetComponent<SmartCameraFOV>().enabled = false;
        _camera.GetComponent<SmartCameraXPosition>().enabled = false;
        _time = 0.0f;
    }

    public void Reset() {
        _initialTarget = new Vector3(0.0f, 0.0f, 0.0f);
    }

    public void SetAdjustSpeed(float speed) {
        _adjustSpeed = speed;
    }
}
