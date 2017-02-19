using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName ="Scripted Camera/New Scripted Camera Handler")]
public class ScriptedCamera : ScriptableObject {

    //The speed at which the FOV will adjust
    [SerializeField]
    protected float _FOVAdjustSpeed = 10.0f;

    private bool largerFOV = false;

    [SerializeField]
    protected float _positionAdjustSpeed = 7.0f;

    protected GameObject _camera;       // Reference to the main Camera
    protected float _time = 0.0f;       // Used for timing and delay purposes

    private Vector3 _initialTarget;

    private void OnEnable() {
        // Get a reference to the main camera to be used with the rest of the script.
        _camera = GameObject.FindGameObjectWithTag("SmartCamera");
        _initialTarget = Vector3.zero;
    }

    public void MoveCamera(Vector3 target) {

        // If adjustment is needed, get the current time and the current orthographic size (aka FOV)
        //_time = _time == 0.0f ? Time.time : _time;
        float fromFOV = Camera.main.orthographicSize;

        if (_initialTarget != target) {
            _time = Time.time;
            _initialTarget = target;
            largerFOV = fromFOV < target.z ? true : false;
        }

        // Using a smooth transition, keep adjusting the fov until the target number has been reached.
        if (largerFOV && Camera.main.orthographicSize < target.z) {
            Camera.main.orthographicSize = Mathf.SmoothStep(fromFOV, target.z + 2.0f, (Time.time - _time) / _FOVAdjustSpeed);
        }
        else if (!largerFOV && Camera.main.orthographicSize > target.z) {
            Camera.main.orthographicSize = Mathf.SmoothStep(fromFOV, target.z - 2.0f, (Time.time - _time) / _FOVAdjustSpeed);
        }

        //if ( (_camera.transform.position.x < _target.x - 0.5f && _camera.transform.position.x > _target.x + 0.5f) || 
        //    (_camera.transform.position.y < _target.y - 0.5f && _camera.transform.position.y > _target.y + 0.5f)) {
        if ((Vector2)_camera.transform.position != (Vector2)target) { 
            _camera.transform.position = new Vector3(Mathf.SmoothStep(_camera.transform.position.x, target.x, (Time.time - _time) / _positionAdjustSpeed),
                Mathf.SmoothStep(_camera.transform.position.y, target.y, (Time.time - _time) / _positionAdjustSpeed), -10.0f);
        }
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
}
