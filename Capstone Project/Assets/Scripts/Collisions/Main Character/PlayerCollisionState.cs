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

    private void OnCollisionEnter2D(Collision2D otherGO) {
        if (otherGO.gameObject.layer == _solidGroundLayer && OnHitGround != null) {
            OnHitGround();
            _onSolidGround = true;
        }
    }

    private void OnCollisionExit2D(Collision2D otherGO) {
        if (otherGO.gameObject.layer == _solidGroundLayer && OnLifted != null) {
            OnLifted();
            _onSolidGround = false;
        }
    }
}
