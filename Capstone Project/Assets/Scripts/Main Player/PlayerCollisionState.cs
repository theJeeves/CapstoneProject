using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class PlayerCollisionState : MonoBehaviour {

    #region Private Fields
    [Header("Collision Varialbes")]
    [SerializeField]
    private LayerMask _whatToHit;

    [SerializeField]
    private Vector2 _rayCastOffset;

    [Space]
    [Header("Effects")]
    [SerializeField]
    private SOEffects _SOEffectHandler;

    private BoxCollider2D m_Box = null;                             // Used to get the dimension of the collider
    private Vector2[] m_RayOrigin = new Vector2[2];          // Stores the left and right side of the collider's world position
    private Vector2 m_Direction = Vector2.zero;                             // Only interested in what is below the player's feet.
    private Rigidbody2D m_Body2d = null;

    private float m_Distance = float.NaN;                                // The distance the raycast will travel will be slightly past the player's feet.
    private bool m_TouchedGround = false;                            // Used to keep track of past states for EVENT purposes.

    #endregion Private Fields

    #region Initializers
    private void OnEnable() {

        m_Box = GetComponent<BoxCollider2D>();
        m_Body2d = GetComponent<Rigidbody2D>();

        // The distance should just be long enough to extend outside of the collider box.
        m_Distance = 2.0f;
        m_Direction = new Vector2(0.0f, -1.0f);              // Down direction.
        m_TouchedGround = false;
    }

    #endregion Initializers

    #region Events
    public static event UnityAction HitGround;
    public static event UnityAction Lifted;

    #endregion Events

    #region Properties
    public bool OnSolidGround { get; private set; } = false;

    #endregion Properties

    #region Private Methods
    private void FixedUpdate() {

        // Get the position of the player's collider box every fixed update
        m_RayOrigin[0] = new Vector2(m_Box.bounds.min.x + _rayCastOffset.x, m_Box.bounds.min.y + 1.0f);
        m_RayOrigin[1] = new Vector2(m_Box.bounds.max.x - _rayCastOffset.x, m_Box.bounds.min.y + 1.0f);

        // For both sides of the player, check if the Raycast is touching the floor.
        // Only check the right side if the player's left side is not touching. Most of the time, 
        // the player will be traveling left to right. Therefore, we should be checking the left side more.
        RaycastHit2D hit;
        for (int i = 0; i < 2; ++i) {
            hit = Physics2D.Raycast(m_RayOrigin[i], m_Direction, m_Distance, _whatToHit);
            if (hit.collider != null)
            {
                Debug.DrawRay(m_RayOrigin[i], new Vector3(0.0f, -1.0f * m_Distance, 0.0f), Color.white);

                m_TouchedGround = m_Body2d.velocity.y <= 2.0f ? true : false;
                break;
            }
        }

        // Send out this event if the player wasn't on the ground and its status has changed.
        if (!OnSolidGround && m_TouchedGround)
        {
            if (SceneManager.GetActiveScene().name == Tags.MainMenuTag)
            {
                GameObject instance = _SOEffectHandler.PlayEffect(EffectEnums.LandingDust, new Vector2(transform.position.x, transform.position.y - 1.0f));
                instance.transform.localScale = new Vector3(0.75f, 0.75f, 1.0f);
            }
            else
            {
                _SOEffectHandler.PlayEffect(EffectEnums.LandingDust, transform.position);
            }
            HitGround?.Invoke();
        }
        // Send out this event if the player was on the ground and its status has changed.
        else if (OnSolidGround && !m_TouchedGround)
        {
            Lifted?.Invoke();
        }

        OnSolidGround = m_TouchedGround ? true : false;         // Update the collision state.
        m_TouchedGround = false;                                 // Reset the collision check.
    }

    #endregion Private Methods
}