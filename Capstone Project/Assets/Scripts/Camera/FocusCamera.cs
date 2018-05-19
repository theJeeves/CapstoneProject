using UnityEngine;

public class FocusCamera : MonoBehaviour
{ 
    #region Constants
    private const string CAM_PATH = "ScriptableObjects/SOScriptedCamHandler";

    #endregion Constants

    #region Private Fields
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

    private ScriptedCamera m_ScriptedCam;
    private Vector2 m_PlayerPos = Vector2.zero;
    private BoxCollider2D m_Collider;

    private int m_Length = 0;
    private float m_MovementTime = 0.0f;
    private float m_DefaultMovementTime = 0.5f;

    #endregion Private Fields

    #region Private Initializers
    private void OnEnable()
    {
        PlayerHealth.UpdateHealth += UponDeath;
        m_Collider = GetComponent<BoxCollider2D>();

        m_ScriptedCam = Resources.Load<ScriptedCamera>(CAM_PATH);
    }

    #endregion Private Initializers

    #region Private Finalizers
    private void OnDisable()
    {
        PlayerHealth.UpdateHealth -= UponDeath;
    }

    #endregion Private Finalizers

    private void OnTriggerEnter2D(Collider2D otherGO)
    {
        bool allowAction = m_Collider.enabled;
        allowAction &= otherGO.tag == Tags.PlayerTag;

        if (allowAction)
        {
            m_ScriptedCam.SetAdjustSpeed(_adjustDuration);
        }
    }

    #region Private Methods
    private void OnTriggerStay2D(Collider2D otherGO)
    {
        bool allowAction = m_Collider.enabled;
        allowAction &= otherGO.tag == Tags.PlayerTag;
        allowAction &= TimeTools.TimeExpired(ref m_MovementTime);

        if (allowAction)
        {
            m_PlayerPos = otherGO.transform.position;
            m_Length = _triggerPoints.Length;

            if (m_Length > 1)
            {
                for (int i = 0; i < m_Length - 1; ++i)
                {
                    if (_leftAndRight)
                    {
                        if (m_PlayerPos.x > _triggerPoints[i].position.x && m_PlayerPos.x < _triggerPoints[i + 1].position.x)
                        {
                            if (_linearMovement.Length > 0 && _linearMovement[i])
                            {
                                float difference = _triggerPoints[i + 1].position.x - _triggerPoints[i].position.x;
                                float percentage = 1.0f - ((_triggerPoints[i + 1].position.x - m_PlayerPos.x) / difference);

                                SetCameraOrigin();
                                m_ScriptedCam.LinearlyMoveCamera(percentage, m_ScriptedCam.GetLinearCamPosition(gameObject.name), _cameraPositions[i]);
                            }
                            else
                            {
                                m_ScriptedCam.MoveCamera(_cameraPositions[i]);
                            }
                        }
                    }
                    else if (_bottomToTop)
                    {
                        if (m_PlayerPos.y > _triggerPoints[i].position.y && m_PlayerPos.y < _triggerPoints[i + 1].position.y) {
                            m_ScriptedCam.MoveCamera(_cameraPositions[i]);
                        }
                    }
                    else if (_topToBottom)
                    {
                        if (m_PlayerPos.y < _triggerPoints[i].position.y && m_PlayerPos.y > _triggerPoints[i + 1].position.y) {
                            m_ScriptedCam.MoveCamera(_cameraPositions[i]);
                        }
                    }
                }
            }
            else if (m_Length == 0)
            {
                m_ScriptedCam.MoveCamera(_cameraPositions[0]);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D otherGO)
    {
        bool allowAction = m_Collider.enabled;
        allowAction &= gameObject.GetComponent<FocusCamera>().enabled;
        allowAction &= otherGO.tag == Tags.PlayerTag;

        if (allowAction)
        {
             m_ScriptedCam.Reset();
        }
    }

    private void UponDeath(int health)
    {
        if (health <= 0)
        {
            m_MovementTime = m_DefaultMovementTime;
        }
    }

    private void SetCameraOrigin()
    {
        if (!m_ScriptedCam.LinearCamPositionSet(gameObject.name))
        { 
            m_ScriptedCam.SetLinearCamPosition(gameObject.name, GameObject.FindGameObjectWithTag(Tags.SmartCameraTag).transform.position);
        }
    }

    #endregion Private Methods
}
