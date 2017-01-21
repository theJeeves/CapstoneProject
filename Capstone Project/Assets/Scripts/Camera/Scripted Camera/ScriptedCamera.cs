using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName ="Scripted Camera/New Scripted Camera")]
public class ScriptedCamera : ScriptableObject {

    // Bool to determine if the script should adjust the FOV.
    [SerializeField]
    protected bool _adjustFOV;

    // Allows the developer to define how close or far the camera should pan in/out.
    [SerializeField, Range(25.0f, 350.0f)]
    protected float _toFOV = 150.0f;

    //The speed at which the FOV will adjust
    [SerializeField]
    protected float _FOVAdjustSpeed = 10.0f;

    // Bool to determine if the script should adjust the X-Position.
    [SerializeField]
    protected bool _adjustXPosition;

    //Allows the developer to define where the player should be in respect to the x value of the screen.
    // 0.0f is the leftmost and 1.0f is the rightmost values.
    [SerializeField, Range(0.0f, 1.0f)]
    protected float _toXPos = 0.5f;

    //The speed at which the X-Position should be adjusted.
    [SerializeField]
    protected float _XAdjustSpeed = 10.0f;

    //Bool to determine if the script should adjust the Y-Position
    [SerializeField]
    protected bool _adjustYPosition;

    //Allows the developer to define where the player should be in repect to the y value of the screen.
    // 0.0f is the upmost and 1.0f is the downmost position on the screen.
    [SerializeField, Range(0.0f, 1.0f)]
    protected float _toYPos = 0.5f;

    //The speed at which the Y-Position should be adjusted.
    [SerializeField]
    protected float _YAdjustSpeed = 10.0f;

    protected GameObject _camera;       // Reference to the main Camera
    protected float _time = 0.0f;       // Used for timing and delay purposes

    // This is used to determine how the camera should move based on if the player will approach this trigger from the
    // left or right side. Just need one boolean to keep track of both.
    private bool _rightTrigger = false;
    public bool IsRightTrigger {
        set { _rightTrigger = value; }
    }

    private bool _fovDone = false;
    private bool _xDone = false;

    private void OnEnable() {
        // Get a reference to the main camera to be used with the rest of the script.
        _camera = GameObject.FindGameObjectWithTag("SmartCamera");
    }

    public bool MoveCamera(Vector2 playerPos) {

        // If adjustment is needed, get the current time and the current orthographic size (aka FOV)
        _time = _time == 0.0f ? Time.time : _time;
        float fromFOV = Camera.main.orthographicSize;

        // Using a smooth transition, keep adjusting the fov until the target number has been reached.
        if (_adjustFOV && Camera.main.orthographicSize < _toFOV) {
            Camera.main.orthographicSize = Mathf.SmoothStep(fromFOV, _toFOV + 2.0f, (Time.time - _time) / _FOVAdjustSpeed);
        }
        else { _fovDone = true; }


        switch (_rightTrigger) {
            
            // In either case, keep adjusting the camera's x-position unitl the target has been reached.
            case true:
                if (_adjustXPosition && Camera.main.WorldToViewportPoint(playerPos).x < 1.0f - _toXPos) {
                    _camera.transform.position += Vector3.left * _XAdjustSpeed;
                }
                else { _xDone = true; }
                break;

            case false:
                if (_adjustXPosition && Camera.main.WorldToViewportPoint(playerPos).x > _toXPos) {
                    _camera.transform.position += Vector3.right * _XAdjustSpeed;
                }
                else { _xDone = true; }
                break;
        }

        //TODO: I STILL NEED TO INCLUDE THE Y-POSITION CODE

        return _fovDone == true && _xDone == true ? true : false;
    }

    //Depending on what the developer wants to do, EnableScripts and DisableScripts will turn on/off
    //when a scripted camera event is needed. 
    public void EnableScripts() {
        if (_adjustFOV) {
            _camera.GetComponent<SmartCameraFOV>().enabled = true;
        }
        if (_adjustXPosition) {
            _camera.GetComponent<SmartCameraXPosition>().enabled = true;
        }
        if (_adjustYPosition) {
            _camera.GetComponent<SmartCameraYPosition>().enabled = true;
        }
        _time = 0.0f;
        _xDone = false;
        _fovDone = false;
    }

    public void DisableScripts() {
        if (_adjustFOV) {
            _camera.GetComponent<SmartCameraFOV>().enabled = false;
        }
        if (_adjustXPosition) {
            _camera.GetComponent<SmartCameraXPosition>().enabled = false;
        }
        if (_adjustYPosition) {
            _camera.GetComponent<SmartCameraYPosition>().enabled = false;
        }
        _time = 0.0f;
        _xDone = false;
        _fovDone = false;
    }
}
