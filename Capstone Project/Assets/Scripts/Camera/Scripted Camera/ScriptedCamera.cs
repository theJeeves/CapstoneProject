using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName ="Scripted Camera/New Scripted Camera Handler")]
public class ScriptedCamera : ScriptableObject {

    #region Fields

    #region Public Fields
    public List<string> keys;
    public List<Vector3> values;
    #endregion Public Fields

    #region Protected Fields
    //The speed at which the FOV will adjust
    [SerializeField]
    protected float _adjustSpeed = 10.0f;

    protected GameObject _camera;       // Reference to the main Camera
    protected float m_timeElapsed = 0.0f;       // Used for timing and delay purposes

    #endregion Protected Fields

    #region Private Fields
    private Vector3 _initialTarget;
    private int _currentIndex = 0;
    private Dictionary<string, Vector3> _linearCamPositions = new Dictionary<string, Vector3>();

    #endregion Private Fields

    #endregion Fields

    #region Private Initializers
    private void OnEnable()
    {
        FindSmartCamera();
        _initialTarget = Vector3.zero;

        _currentIndex = 0;
        for (int i = 0; i < keys.Count; ++i)
        {
            if (values[i] != Vector3.zero)
            {
                _currentIndex = i;
            }
            else {
                if (_currentIndex == 0)
                {
                    _currentIndex = -1;
                }
                break;
            }
        }

        if (_currentIndex != -1)
        {
            for (int i = 0; i < _currentIndex + 1; ++i)
            {
                _linearCamPositions.Add(keys[i], values[i]);
            }
        }
        else
        {
            _currentIndex = 0;
        }
    }

    #endregion Private Initializers

    #region Public Methods

    /// <summary>
    /// Move the camera to a specified position.
    /// </summary>
    /// <param name="target"></param>
    public void MoveCamera(Vector3 target)
    {
        if (_initialTarget != target)
        {
            m_timeElapsed = 0.0f;
            _initialTarget = target;
        }

        FindSmartCamera();

        if (_camera.transform.position != target)
        {
            TimeTools.TimeElapsed(ref m_timeElapsed);
            _camera.transform.position = new Vector3(Mathf.SmoothStep(_camera.transform.position.x, target.x, m_timeElapsed / _adjustSpeed),
                Mathf.SmoothStep(_camera.transform.position.y, target.y, m_timeElapsed / _adjustSpeed),
                Mathf.SmoothStep(_camera.transform.position.z, target.z, m_timeElapsed / _adjustSpeed));
        }
    }

    /// <summary>
    /// Move the camera linearly from one position to another.
    /// </summary>
    /// <param name="percentage"></param>
    /// <param name="originPos"></param>
    /// <param name="target"></param>
    public void LinearlyMoveCamera(float percentage, Vector3 originPos, Vector3 target)
    {
        FindSmartCamera();
        _camera.transform.position = new Vector3(percentage * (target.x - originPos.x) + originPos.x, 
                                                    percentage * (target.y - originPos.y) + originPos.y, 
                                                    percentage * (target.z - originPos.z) + originPos.z);
    }

    /// <summary>
    /// Enable the camera to move with the player on its own.
    /// </summary>
    public void EnableScripts()
    {
        _camera.GetComponent<SmartCameraXPosition>().enabled = true;
        m_timeElapsed = 0.0f;
    }

    /// <summary>
    /// Disable the camera from moving with the player on its own.
    /// </summary>
    public void DisableScripts()
    {
        _camera.GetComponent<SmartCameraXPosition>().enabled = false;
        m_timeElapsed = 0.0f;
    }

    /// <summary>
    /// Reset the camera to the origin of the parent object.
    /// </summary>
    public void Reset() {
        _initialTarget = Vector3.zero;
    }

    /// <summary>
    /// Set the speed at which the camera will adjust.
    /// </summary>
    /// <param name="speed"></param>
    public void SetAdjustSpeed(float speed)
    {
        _adjustSpeed = speed;
    }

    /// <summary>
    /// Determine if the camera is set to move linearly.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public bool LinearCamPositionSet(string key)
    {
        return _linearCamPositions.ContainsKey(key);
    }

    /// <summary>
    /// Set the camera position so it may move linearly.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void SetLinearCamPosition(string key, Vector3 value)
    {
        if (!_linearCamPositions.ContainsKey(key))
        {
            keys[_currentIndex] = key;
            values[_currentIndex++] = value;
            _linearCamPositions.Add(key, value);
        }
    }

    /// <summary>
    /// Return the position as a Vector3 for a linearly set camera spot.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public Vector3 GetLinearCamPosition(string key)
    {
        return _linearCamPositions.ContainsKey(key) ? _linearCamPositions[key] : Vector3.zero;
    }

    #endregion Public Methods

    #region Private Methods
    private void FindSmartCamera() {
        // Get a reference to the main camera to be used with the rest of the script.
        if (_camera == null)
        {
            _camera = GameObject.FindGameObjectWithTag(Tags.SmartCameraTag);
        }
    }
    #endregion Private Methods
}