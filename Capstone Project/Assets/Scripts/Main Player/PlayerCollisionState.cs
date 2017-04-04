using UnityEngine;
using System.Collections;
using SpriterDotNetUnity;
using UnityEngine.SceneManagement;

public class PlayerCollisionState : MonoBehaviour {

    public delegate void PlayerCollisionStateEvent();
    public static event PlayerCollisionStateEvent OnHitGround;
    public static event PlayerCollisionStateEvent OnLifted;

    [Header("Collision Varialbes")]
    [SerializeField]
    private LayerMask _whatToHit;

    private bool _onSolidGround;
    public bool OnSolidGround {
        get { return _onSolidGround; }
    }

    [SerializeField]
    private Vector2 _rayCastOffset;

    [Space]

    [Header("Effects")]

    [SerializeField]
    private SOEffects _SOEffectHandler;

    private BoxCollider2D _box;                             // Used to get the dimension of the collider
    //private PolygonCollider2D _box;
    private Vector2[] _rayOrigin = new Vector2[2];          // Stores the left and right side of the collider's world position
    private float _distance;                                // The distance the raycast will travel will be slightly past the player's feet.
    private Vector2 _direction;                             // Only interested in what is below the player's feet.
    private bool _touchedGround;                            // Used to keep track of past states for EVENT purposes.

    private Vector2[] _notAiming = { new Vector2(1.57f, 6.11f), new Vector2(4.35f, 3.3f),
                                    new Vector2(2.22f, 0.0f), new Vector2(-0.34f, 0.0f),
                                    new Vector2(-2.04f, 5.04f)};

    private Vector2[] _aiming = { new Vector2(1.57f, 6.11f), new Vector2(6.35f, 3.3f),
                                    new Vector2(2.22f, 0.0f), new Vector2(-0.34f, 0.0f),
                                    new Vector2(-2.04f, 5.04f)};

    //private ControllableObject _controller;
    private Rigidbody2D _body2d;

    private void OnEnable() {

        _box = GetComponent<BoxCollider2D>();
        //_box = GetComponent<PolygonCollider2D>();
        //_controller = GetComponent<ControllableObject>();
        _body2d = GetComponent<Rigidbody2D>();

        // The distance should just be long enough to extend outside of the collider box.
        _distance = 2.0f;
        _direction = new Vector2(0.0f, -1.0f);              // Down direction.
        _touchedGround = false;
    }

    private void FixedUpdate() {

        // Get the position of the player's collider box every fixed update
        _rayOrigin[0] = new Vector2(_box.bounds.min.x + _rayCastOffset.x, _box.bounds.min.y + 1.0f);
        _rayOrigin[1] = new Vector2(_box.bounds.max.x - _rayCastOffset.x, _box.bounds.min.y + 1.0f);

        //// Get the position of the player's collider box every fixed update
        //if ((_controller.GetButtonPress(Buttons.AimRight) || _controller.GetButtonPress(Buttons.AimLeft))) {
        //    _rayOrigin[0] = new Vector2(_box.bounds.center.x * -(float)_controller.FacingDirection, _box.bounds.min.y + 1.0f);
        //    _rayOrigin[1] = new Vector2(_box.bounds.center.x + 13.5f * -(float)_controller.FacingDirection, _box.bounds.min.y + 1.0f);
        //}
        //else {
        //    _rayOrigin[0] = new Vector2(_box.bounds.center.x + 5.0f * (float)_controller.FacingDirection, _box.bounds.min.y + 1.0f);
        //    _rayOrigin[1] = new Vector2(_box.bounds.center.x - 7.75f * (float)_controller.FacingDirection, _box.bounds.min.y + 1.0f);
        //}

        // For both sides of the player, check if the Raycast is touching the floor.
        // Only check the right side if the player's left side is not touching. Most of the time, 
        // the player will be traveling left to right. Therefore, we should be checking the left side more.
        RaycastHit2D hit;
        for (int i = 0; i < 2; ++i) {
            hit = Physics2D.Raycast(_rayOrigin[i], _direction, _distance, _whatToHit);
            if (hit.collider != null) {
                Debug.DrawRay(_rayOrigin[i], new Vector3(0.0f, -1.0f * _distance, 0.0f), Color.white);

                _touchedGround = _body2d.velocity.y <= 2.0f ? true : false;
                break;
            }
        }

        // Send out this event if the player wasn't on the ground and its status has changed.
        if (!_onSolidGround && _touchedGround) {

            if (SceneManager.GetActiveScene().name == "Main Menu") {
                GameObject instance = _SOEffectHandler.PlayEffect(EffectEnums.LandingDust, new Vector2(transform.position.x, transform.position.y - 1.0f));
                instance.transform.localScale = new Vector3(0.75f, 0.75f, 1.0f);
            }
            else {
                _SOEffectHandler.PlayEffect(EffectEnums.LandingDust, transform.position);
            }
            if (OnHitGround != null) {
                OnHitGround();
            }
        }
        // Send out this event if the player was on the ground and its status has changed.
        else if (_onSolidGround && !_touchedGround) {
            if (OnLifted != null) {
                OnLifted();
            }
        }

        _onSolidGround = _touchedGround ? true : false;         // Update the collision state.
        _touchedGround = false;                                 // Reset the collision check.
    }
}