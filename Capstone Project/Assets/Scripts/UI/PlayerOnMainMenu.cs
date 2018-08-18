using UnityEngine;
using SpriterDotNetUnity;
using System.Collections.Generic;
using System.Linq;

public class PlayerOnMainMenu : MonoBehaviour {

    #region Private Fields
    [SerializeField]
    private MovementRequest _walkingMovementRequest;

    private ControllableObject m_Controller = null;
    private PlayerCollisionState m_CollisionState = null;
    private UnityAnimator m_SpriterAnimator = null;
    private List<string> m_AnimationList = null;
    private int m_CurrentIndex = 38;
    private int m_PreviousIndex = -100;
    private XFloat m_Time = 1.0f;

    #endregion Private Fields

    #region Initializers
    private void OnEnable()
    {
        m_CollisionState = GetComponent<PlayerCollisionState>();
        m_Controller = GetComponent<ControllableObject>();

        // START THE GAME MANAGER, WINDOW MANAGER, AND THE EVENT SYSTEM
        // They are not used in this script, but they started for the reset of the game.
        GameManager.Instance.GetComponent<GameManager>() ;
        WindowManager.Instance.GetComponent<WindowManager>();
        EventSystemSingleton.Instance.GetComponent<EventSystemSingleton>();
    }

    #endregion Initializers

    #region Private Methods
    private void Update()
    {
        if (m_SpriterAnimator == null)
        {
            m_SpriterAnimator = GetComponent<SpriterDotNetBehaviour>().Animator;
            m_AnimationList = m_SpriterAnimator.GetAnimations().ToList();
        }

        if (m_CollisionState.OnSolidGround)
        {
            if (m_Time.IsExpired)
            {
                m_Controller.SetButtonPressTime(Buttons.MoveRight);
                _walkingMovementRequest.RequestMovement(Buttons.MoveRight);
                if (m_CurrentIndex != m_PreviousIndex)
                {
                    Play(m_CurrentIndex);
                    m_PreviousIndex = m_CurrentIndex;
                }
            }
        }
        else
        {
            m_Time.Reset();
        }
    }

    private void Play(int index)
    {
        m_SpriterAnimator.Play(m_AnimationList[index]);
    }

    #endregion Private Methods
}
