using UnityEngine;
using System.Collections;

//[System.Serializable]
//public struct ContactPoint {
//    [SerializeField]
//    private string _pointName;
//    public string PointName {
//        get { return _pointName; }
//    }

//    [SerializeField]
//    private Vector2 _position;
//    public Vector2 Position {
//        get { return _position; }
//    }

//    [SerializeField]
//    private LayerMask _layerMask;
//    public LayerMask LayerMask {
//        get { return _layerMask; }
//    }

//    [SerializeField]
//    private bool _touching;
//    public bool Touching {
//        get { return _touching; }
//        set { _touching = value; }
//    }
//}

public class PlayerCollisionState : MonoBehaviour {

    //// Cannot use an array of LayerMasks. When I tried it, all that would return would be the
    //// number of available LayerMasks and not the values assigned to the array in Inspector.
    [SerializeField]
    private LayerMask _headLayer;
    [SerializeField]
    private LayerMask _solidGroundLayer;

    [SerializeField]
    private Vector2 _headPoint = Vector2.zero;
    [SerializeField]
    private Vector2 _feetPoint = Vector2.zero;

    [SerializeField]
    private float _radius = 0.0f;

    [SerializeField]
    private bool _headColiision;
    public bool HeadCollision {
        get { return _headColiision; }
    }

    [SerializeField]
    private bool _onSolidGround;
    public bool OnSolidGround {
        get { return _onSolidGround; }
    }

    private void FixedUpdate() {

        //foreach (ContactPoint point in _points) {
        //    UpdateState(point);
        //}

        Vector2 point = _headPoint;
        point.x += transform.position.x;
        point.y += transform.position.y;

        _headColiision = Physics2D.OverlapCircle(point, _radius, _headLayer);

        point = _feetPoint;
        point.x += transform.position.x;
        point.y += transform.position.y;

        _onSolidGround = Physics2D.OverlapCircle(point, _radius, _solidGroundLayer);
    }

    //private void UpdateState(ContactPoint contactPoint) {

    //    Vector2 point = contactPoint.Position;

    //    point.x += transform.position.x;
    //    point.y += transform.position.y;

    //    contactPoint.Touching = Physics2D.OverlapCircle(contactPoint.Position, _radius, contactPoint.LayerMask);
    //}

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Vector2[] positions = new Vector2[] { _headPoint, _feetPoint };

        foreach(Vector2 position in positions ) {
            Vector2 pos = position;
            pos.x += transform.position.x;
            pos.y += transform.position.y;

            Gizmos.DrawWireSphere(pos, _radius);
        }
    }
}
