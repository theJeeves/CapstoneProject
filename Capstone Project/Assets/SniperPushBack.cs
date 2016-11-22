using UnityEngine;
using System.Collections;

public class SniperPushBack : MonoBehaviour {

    public delegate void SniperPushBackEvent();
    public static event SniperPushBackEvent Stun;

    private Rigidbody2D _body2d;
    float _direction;

    private void OnTriggerEnter2D(Collider2D go) {
        Debug.Log("triggered");

        if (go.gameObject.tag == "Player") {
            if (Stun != null) {
                Stun();
            }
            _body2d = go.GetComponent<Rigidbody2D>();
            _direction = transform.localScale.x > 0 ? 1.0f : -1.0f;
            _body2d.AddForce(new Vector2(1.0f, 0.0f) * 25000.0f * _direction, ForceMode2D.Impulse);
        }
    }
}
