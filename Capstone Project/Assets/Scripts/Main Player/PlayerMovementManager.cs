using UnityEngine;
using System.Collections.Generic;

public class PlayerMovementManager : MonoBehaviour {

    #region Fields
    private Rigidbody2D m_Body2d;
    private ControllableObject m_Controller;
    private PlayerCollisionState m_CollisionState;

    private bool m_Grounded = false;
    private Vector3 m_Values;
    private float m_Time = 0.0f;

    private AudioSource m_AudioSource;
    private bool m_IsPlaying = false;

    // The key is whether or not the movement should be additive or override the current movement.
    private Queue<MovementRequest> m_MoveQ = new Queue<MovementRequest>();
    #endregion Fields

    #region Initializers
    private void OnEnable() {
        m_Body2d = GetComponent<Rigidbody2D>();
        m_Controller = GetComponent<ControllableObject>();
        m_CollisionState = GetComponent<PlayerCollisionState>();
        m_AudioSource = GetComponent<AudioSource>();
    }
    #endregion Initializers

    #region Public Methods
    public void Enqueue(MovementRequest request) {
        m_MoveQ.Enqueue(request);
    }

    //Instantly move the player (Mainly for world/enemy factors)
    public void AddImpulseForce(Vector2 force) {
        m_Body2d.AddForce(force, ForceMode2D.Impulse);
    }

    public void ClearQueue() {
        m_MoveQ.Clear();
    }
    #endregion Public Methods

    #region Private Methods

    // Update is called once per frame
    private void Update () {

        m_Grounded = m_CollisionState.OnSolidGround;

        if (m_MoveQ.Count == 0 && m_Grounded && Mathf.Abs(m_Body2d.velocity.x) > 0.5f) {
            m_Body2d.velocity = new Vector2(Mathf.SmoothStep(m_Body2d.velocity.x, 0.0f, TimeTools.TimeElapsed(ref m_Time) / 1.00f), m_Body2d.velocity.y);
            m_AudioSource.Stop();
            m_IsPlaying = false;
        }
        else {
            m_Time = 0.0f;
        }

        while (m_MoveQ.Count > 0) {
            m_Values = new Vector3(m_Body2d.velocity.x, m_Body2d.velocity.y, m_Controller.GetButtonPressTime(m_MoveQ.Peek().Button));

            switch (m_MoveQ.Peek().MovementType) {
                case MovementType.MainMenuWalking:
                case MovementType.Walking:
                    m_Body2d.velocity = m_MoveQ.Dequeue().Move(m_Values, m_Grounded, m_Controller.CurrentKey);
                    if (!m_IsPlaying) {
                        m_AudioSource.Play();
                        m_IsPlaying = true;
                    }
                    break;
                case MovementType.Shotgun:
                case MovementType.MachineGun:
                    m_Body2d.velocity = m_MoveQ.Dequeue().Move(m_Values, m_Grounded, m_Controller.CurrentKey);
                    break;
                case MovementType.AddForce:
                    m_MoveQ.Dequeue().Move(Vector3.zero, false, m_Controller.CurrentKey);
                    break;
                default:
                    m_MoveQ.Dequeue();
                    Debug.LogError("Movement Type Not Defined.");
                    break;
            }
        }
    }
    #endregion Private Methods
}
