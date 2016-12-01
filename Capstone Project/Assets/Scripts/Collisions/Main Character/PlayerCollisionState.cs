using UnityEngine;
using System.Collections;

public class PlayerCollisionState : MonoBehaviour {

    public delegate void PlayerCollisionStateEvent();
    public static event PlayerCollisionStateEvent OnHitGround;
    public static event PlayerCollisionStateEvent OnLifted;

    //// Cannot use an array of LayerMasks. When I tried it, all that would return would be the
    //// number of available LayerMasks and not the values assigned to the array in Inspector.

    // Variables for the bottom of the character
    [SerializeField]
    private LayerMask _solidGroundLayer;

    [SerializeField]
    private bool _onSolidGround;
    public bool OnSolidGround {
        get { return _onSolidGround; }
    }

    private BoxCollider2D _box;

    private void Awake() {
        _box = GetComponent<BoxCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D otherGO) {

        // Ensure the player is on something they can walk on and ensure the platform / floor
        // is under them for proper reloading. 
        if (otherGO.gameObject.tag == "SolidGround" && 
            otherGO.transform.position.y < _box.bounds.min.y) {
            _onSolidGround = true;

            if (OnHitGround != null) {
                OnHitGround();
            }
        }
    }

    private void OnCollisionExit2D(Collision2D otherGO) {
        if (otherGO.gameObject.tag == "SolidGround") {
            _onSolidGround = false;

            if (OnLifted != null) {
                OnLifted();
            }
        }
    }
}
