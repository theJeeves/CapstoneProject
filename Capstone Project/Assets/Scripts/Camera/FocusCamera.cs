using UnityEngine;

public class FocusCamera : MonoBehaviour {

    #region Constant Fields
    private const string PLAYER_TAG = "Player";
    #endregion Constant Fields

    #region Fields
    private ScriptedCamera _scriptedCam;

    [Header("Movement Type")]
    [SerializeField]
    private bool m_LeftAndRight = true;
    [SerializeField]
    private bool m_TopToBottom = false;
    [SerializeField]
    private bool m_BottomToTop = false;

    [Space]

    [SerializeField]
    private float m_AdjustDuration = 3.0f;

    [SerializeField]
    private Transform[] m_TriggerPoints;
    [SerializeField]
    private Vector3[] m_CameraPositions;
    [SerializeField]
    private bool[] m_LinearMovement;

    private Vector2 m_PlayerPos = Vector2.zero;
    private int m_Length = 0;
    private float m_MovementTime = 0.0f;
    private float m_DefaultMovementTime = 0.5f;

    private Vector3 m_CamOrigin = Vector3.zero;
    private bool m_OriginSet = false;

    private BoxCollider2D m_Collider;
    #endregion Fields

    #region Initializers
    private void OnEnable() {
        PlayerHealth.UpdateHealth += UponDeath;
        m_Collider = GetComponent<BoxCollider2D>();

        _scriptedCam = Resources.Load<ScriptedCamera>("ScriptableObjects/SOScriptedCamHandler");
    }
    #endregion Initializers

    #region Finalizers
    private void OnDisable() {
        PlayerHealth.UpdateHealth -= UponDeath;
    }
    #endregion Finalizers

    #region Private Methods
    private void OnTriggerEnter2D(Collider2D otherGO) {
        if (m_Collider.enabled && otherGO.tag == PLAYER_TAG) {
            _scriptedCam.SetAdjustSpeed(m_AdjustDuration);
        }
    }

    private void OnTriggerStay2D(Collider2D otherGO) {

        bool allowAction = m_Collider.enabled;
        allowAction &= otherGO.tag == PLAYER_TAG;
        allowAction &= TimeTools.TimeExpired(ref m_MovementTime);

        if (allowAction) {

            m_PlayerPos = otherGO.transform.position;
            m_Length = m_TriggerPoints.Length;

            if (m_Length > 1) {
                for (int i = 0; i < m_Length - 1; ++i) {

                    if (m_LeftAndRight) {
                        if (m_PlayerPos.x > m_TriggerPoints[i].position.x && m_PlayerPos.x < m_TriggerPoints[i + 1].position.x) {

                            if (m_LinearMovement.Length > 0 && m_LinearMovement[i]) {

                                float difference = m_TriggerPoints[i + 1].position.x - m_TriggerPoints[i].position.x;
                                float percentage = 1.0f - ((m_TriggerPoints[i + 1].position.x - m_PlayerPos.x) / difference);

                                SetCameraOrigin();
                                _scriptedCam.LinearlyMoveCamera(percentage, _scriptedCam.GetLinearCamPosition(gameObject.name), m_CameraPositions[i]);
                            }
                            else {
                                _scriptedCam.MoveCamera(m_CameraPositions[i]);
                            }
                        }
                    }
                    else if (m_BottomToTop) {
                        if (m_PlayerPos.y > m_TriggerPoints[i].position.y && m_PlayerPos.y < m_TriggerPoints[i + 1].position.y) {
                            _scriptedCam.MoveCamera(m_CameraPositions[i]);
                        }
                    }
                    else if (m_TopToBottom) {
                        if (m_PlayerPos.y < m_TriggerPoints[i].position.y && m_PlayerPos.y > m_TriggerPoints[i + 1].position.y) {
                            _scriptedCam.MoveCamera(m_CameraPositions[i]);
                        }
                    }
                }
            }
            else if (m_Length == 0) {
                _scriptedCam.MoveCamera(m_CameraPositions[0]);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D otherGO) {

        bool allowAction = m_Collider.enabled;
        allowAction &= gameObject.GetComponent<FocusCamera>().enabled;

        if (allowAction) {

            if (otherGO.tag == "Player") {
                _scriptedCam.Reset();
            }
        }
    }

    private void UponDeath(int health) {
        if (health <= 0) {
            m_MovementTime = m_DefaultMovementTime;
        }
    }

    private void SetCameraOrigin() {

        if (!_scriptedCam.LinearCamPositionSet(gameObject.name)) { 
            _scriptedCam.SetLinearCamPosition(gameObject.name, GameObject.FindGameObjectWithTag("SmartCamera").transform.position);
            m_OriginSet = true;
        }
    }
    #endregion Private Methods
}
