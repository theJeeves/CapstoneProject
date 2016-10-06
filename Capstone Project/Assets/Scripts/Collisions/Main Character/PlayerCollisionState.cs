﻿using UnityEngine;
using System.Collections;

public class PlayerCollisionState : MonoBehaviour {

    //// Cannot use an array of LayerMasks. When I tried it, all that would return would be the
    //// number of available LayerMasks and not the values assigned to the array in Inspector.

    // Variables for the top of the character
    [SerializeField]
    private LayerMask _headLayer;
    [SerializeField]
    private Vector2 _headPosition = Vector2.zero;
    [SerializeField]
    private bool _headColiision;
    public bool HeadCollision {
        get { return _headColiision; }
    }

    // Variables for the bottom of the character
    [SerializeField]
    private LayerMask _solidGroundLayer;

    [SerializeField]
    private Vector2 _feetPosition = Vector2.zero;

    [SerializeField]
    private bool _onSolidGround;
    public bool OnSolidGround {
        get { return _onSolidGround; }
    }

    // The radius size of each trigger
    [SerializeField]
    private float _radius = 0.0f;

    private void FixedUpdate() {

        Vector2 point = _headPosition;
        point.x += transform.position.x;
        point.y += transform.position.y;

        _headColiision = Physics2D.OverlapCircle(point, _radius, _headLayer);

        point = _feetPosition;
        point.x += transform.position.x;
        point.y += transform.position.y;

        _onSolidGround = Physics2D.OverlapCircle(point, _radius, _solidGroundLayer);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;

        Vector2[] positions = new Vector2[] { _headPosition, _feetPosition };      

        foreach(Vector2 position in positions ) {
            Vector2 pos = position;
            pos.x += transform.position.x;
            pos.y += transform.position.y;

            Gizmos.DrawWireSphere(pos, _radius);
        }
    }
}
