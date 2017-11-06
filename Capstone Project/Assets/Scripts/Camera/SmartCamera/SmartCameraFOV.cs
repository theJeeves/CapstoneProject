using UnityEngine;
using System.Collections;

public class SmartCameraFOV : MonoBehaviour {
    [SerializeField]
    private float _adjustSpeed;
    [SerializeField]
    private float _targetZ = -100.0f;
    private bool _adjust = true;

    private Vector3 _currPosition = Vector3.zero;
    private float _curreVelocity;

    private float m_timeElapsed;

    private bool _decraseing;
    private bool _increasing;

    private void OnEnable() {
        m_timeElapsed = 0.0f;
    }
	
	// Update is called once per frame
	private void LateUpdate () {

        if (_adjust) {

            _currPosition = transform.position;

            if (_targetZ >= _currPosition.z - 1.0f || _targetZ <= _currPosition.z + 1.0f) {
                _adjust = false;
            }
        else {
                transform.position = new Vector3(_currPosition.x, _currPosition.y,
                    Mathf.SmoothStep(_currPosition.z, _targetZ - 0.1f, TimeTools.TimeElapsed(ref m_timeElapsed) / _adjustSpeed));
            }
        }
    }

    public void SetZCamera(float target) {
        _adjust = true;
        _targetZ = target;
    }
}
