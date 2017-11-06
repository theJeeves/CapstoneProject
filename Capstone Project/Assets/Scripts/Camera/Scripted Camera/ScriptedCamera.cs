using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName ="Scripted Camera/New Scripted Camera Handler")]
public class ScriptedCamera : ScriptableObject {

    //The speed at which the FOV will adjust
    [SerializeField]
    protected float _adjustSpeed = 10.0f;

    //private bool largerFOV = false;

    protected GameObject _camera;       // Reference to the main Camera
    protected float m_timeElapsed = 0.0f;       // Used for timing and delay purposes

    private Vector3 _initialTarget;

    private int _currentIndex = 0;
    public List<string> keys;
    public List<Vector3> values;
    private Dictionary<string, Vector3> _linearCamPositions = new Dictionary<string, Vector3>();

    private void OnEnable() {
        // Get a reference to the main camera to be used with the rest of the script.
        _camera = GameObject.FindGameObjectWithTag("SmartCamera");
        _initialTarget = Vector3.zero;

        _currentIndex = 0;
        for (int i = 0; i < keys.Count; ++i) {
            if (values[i] != Vector3.zero) {
                _currentIndex = i;
            }
            else {
                if (_currentIndex == 0) { _currentIndex = -1; }
                break;
            }
        }

        if (_currentIndex != -1) {
            for (int i = 0; i < _currentIndex + 1; ++i) {
                _linearCamPositions.Add(keys[i], values[i]);
            }
        }
        else {
            _currentIndex = 0;
        }
    }

    public void MoveCamera(Vector3 target) {

        if (_initialTarget != target) {
            m_timeElapsed = 0.0f;
            _initialTarget = target;
        }

        if (_camera == null) {
            // Get a reference to the main camera to be used with the rest of the script.
            _camera = GameObject.FindGameObjectWithTag("SmartCamera");
        }

        if (_camera.transform.position != target) {
            TimeTools.TimeElapsed(ref m_timeElapsed);
            _camera.transform.position = new Vector3(Mathf.SmoothStep(_camera.transform.position.x, target.x, m_timeElapsed / _adjustSpeed),
                Mathf.SmoothStep(_camera.transform.position.y, target.y, m_timeElapsed / _adjustSpeed),
                Mathf.SmoothStep(_camera.transform.position.z, target.z, m_timeElapsed / _adjustSpeed));
        }
    }

    public void LinearlyMoveCamera(float percentage, Vector3 originPos, Vector3 target) {

        if (_camera == null) { _camera = GameObject.FindGameObjectWithTag("SmartCamera"); }

            _camera.transform.position = new Vector3(percentage * (target.x - originPos.x) + originPos.x, 
                                                     percentage * (target.y - originPos.y) + originPos.y, 
                                                     percentage * (target.z - originPos.z) + originPos.z);
    }

    //Depending on what the developer wants to do, EnableScripts and DisableScripts will turn on/off
    //when a scripted camera event is needed. 
    public void EnableScripts() {
        //_camera.GetComponent<SmartCameraFOV>().enabled = true;
        _camera.GetComponent<SmartCameraXPosition>().enabled = true;
        m_timeElapsed = 0.0f;
    }

    public void DisableScripts() {
        //_camera.GetComponent<SmartCameraFOV>().enabled = false;
        _camera.GetComponent<SmartCameraXPosition>().enabled = false;
        m_timeElapsed = 0.0f;
    }

    public void Reset() {
        _initialTarget = new Vector3(0.0f, 0.0f, 0.0f);
    }

    public void SetAdjustSpeed(float speed) {
        _adjustSpeed = speed;
    }

    public bool LinearCamPositionSet(string key) {
        return _linearCamPositions.ContainsKey(key);
    }

    public void SetLinearCamPosition(string key, Vector3 value) {
        if (!_linearCamPositions.ContainsKey(key)) {
            keys[_currentIndex] = key;
            values[_currentIndex++] = value;
            _linearCamPositions.Add(key, value);
        }
    }

    public Vector3 GetLinearCamPosition(string key) {
        if (_linearCamPositions.ContainsKey(key)) {
            return _linearCamPositions[key];
        }
        else {
            return Vector3.zero;
        }
    }
}
