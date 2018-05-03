using UnityEngine;

public class FocusCamera : MonoBehaviour {

    #region Private Fields
    private ScriptedCamera _scriptedCam;

    [Header("Movement Type")]
    [SerializeField]
    private bool _leftAndRight = true;
    [SerializeField]
    private bool _topToBottom = false;
    [SerializeField]
    private bool _bottomToTop = false;

    [Space]

    [SerializeField]
    private float _adjustDuration = 3.0f;

    [SerializeField]
    private Transform[] _triggerPoints;
    [SerializeField]
    private Vector3[] _cameraPositions;
    [SerializeField]
    private bool[] _linearMovement;

    private Vector2 playerPos = Vector2.zero;
    private int length = 0;
    private float m_movementTime = 0.0f;
    private float m_defaultMovementTime = 0.5f;
    private Vector3 _camOrigin = Vector3.zero;
    private bool _originSet = false;
    private BoxCollider2D _collider;

    #endregion Private Fields

    #region Private Initializers
    private void OnEnable() {
        PlayerHealth.UpdateHealth += UponDeath;
        _collider = GetComponent<BoxCollider2D>();

        _scriptedCam = Resources.Load<ScriptedCamera>("ScriptableObjects/SOScriptedCamHandler");
    }

    #endregion Private Initializers

    #region Private Finalizers
    private void OnDisable() {
        PlayerHealth.UpdateHealth -= UponDeath;
    }

    #endregion Private Finalizers

    private void OnTriggerEnter2D(Collider2D otherGO) {

        bool allowAction = _collider.enabled;
        allowAction &= otherGO.tag == StringConstantUtility.PLAYER_TAG;

        if (allowAction) {
            _scriptedCam.SetAdjustSpeed(_adjustDuration);
        }
    }

    #region Private Methods
    private void OnTriggerStay2D(Collider2D otherGO) {

        bool allowAction = _collider.enabled;
        allowAction &= otherGO.tag == StringConstantUtility.PLAYER_TAG;
        allowAction &= TimeTools.TimeExpired(ref m_movementTime);

        if (allowAction) {

            playerPos = otherGO.transform.position;
            length = _triggerPoints.Length;

            if (length > 1) {
                for (int i = 0; i < length - 1; ++i) {

                    if (_leftAndRight) {
                        if (playerPos.x > _triggerPoints[i].position.x && playerPos.x < _triggerPoints[i + 1].position.x) {

                            if (_linearMovement.Length > 0 && _linearMovement[i]) {

                                float difference = _triggerPoints[i + 1].position.x - _triggerPoints[i].position.x;
                                float percentage = 1.0f - ((_triggerPoints[i + 1].position.x - playerPos.x) / difference);

                                SetCameraOrigin();
                                _scriptedCam.LinearlyMoveCamera(percentage, _scriptedCam.GetLinearCamPosition(gameObject.name), _cameraPositions[i]);
                            }
                            else {
                                _scriptedCam.MoveCamera(_cameraPositions[i]);
                            }
                        }
                    }
                    else if (_bottomToTop) {
                        if (playerPos.y > _triggerPoints[i].position.y && playerPos.y < _triggerPoints[i + 1].position.y) {
                            _scriptedCam.MoveCamera(_cameraPositions[i]);
                        }
                    }
                    else if (_topToBottom) {
                        if (playerPos.y < _triggerPoints[i].position.y && playerPos.y > _triggerPoints[i + 1].position.y) {
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

        bool allowAction = _collider.enabled;
        allowAction &= gameObject.GetComponent<FocusCamera>().enabled;
        allowAction &= otherGO.tag == StringConstantUtility.PLAYER_TAG;

        if (allowAction) {
             _scriptedCam.Reset();
        }
    }

    private void UponDeath(object sender, int health) {
        if (health <= 0) {
            m_movementTime = m_defaultMovementTime;
        }
    }

    private void SetCameraOrigin() {

        if (!_scriptedCam.LinearCamPositionSet(gameObject.name)) { 
            _scriptedCam.SetLinearCamPosition(gameObject.name, GameObject.FindGameObjectWithTag(StringConstantUtility.SMART_CAMERA_TAG).transform.position);
            _originSet = true;
        }
    }

    #endregion Private Methods
}
