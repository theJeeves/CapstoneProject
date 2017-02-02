using UnityEngine;
using System.Collections;
using SpriterDotNetUnity;

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
    private SOEffects _SOEffect;

    private BoxCollider2D _box;                             // Used to get the dimension of the collider
    private Vector2[] _rayOrigin = new Vector2[2];          // Stores the left and right side of the collider's world position
    private float _distance;                                // The distance the raycast will travel will be slightly past the player's feet.
    private Vector2 _direction;                             // Only interested in what is below the player's feet.
    private bool _touchedGround;                            // Used to keep track of past states for EVENT purposes.

    private void OnEnable() {
        _box = GetComponent<BoxCollider2D>();

        // The distance should just be long enough to extend outside of the collider box.
        _distance = 2.0f;
        _direction = new Vector2(0.0f, -1.0f);              // Down direction.
        _touchedGround = false;
    }

    private void FixedUpdate() {

        // Get the position of the player's collider box every fixed update
        _rayOrigin[0] = new Vector2(_box.bounds.min.x + _rayCastOffset.x, _box.bounds.min.y + 1.0f);
        _rayOrigin[1] = new Vector2(_box.bounds.max.x - _rayCastOffset.x, _box.bounds.min.y + 1.0f);

        // For both sides of the player, check if the Raycast is touching the floor.
        // Only check the right side if the player's left side is not touching. Most of the time, 
        // the player will be traveling left to right. Therefore, we should be checking the left side more.
        RaycastHit2D hit;
        for (int i = 0; i < 2; ++i) {
            hit = Physics2D.Raycast(_rayOrigin[i], _direction, _distance, _whatToHit);
            if (hit.collider != null) {
                Debug.DrawRay(_rayOrigin[i], new Vector3(0.0f, -1.0f * _distance, 0.0f), Color.white);
                _touchedGround = true;
                break;
            }
        }

        // Send out this event if the player wasn't on the ground and its status has changed.
        if (!_onSolidGround && _touchedGround) {
            _SOEffect.PlayVisualEffect(transform.position);
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
