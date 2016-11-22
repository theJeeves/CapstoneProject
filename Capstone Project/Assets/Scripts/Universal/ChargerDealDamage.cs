using UnityEngine;
using System.Collections;

public class ChargerDealDamage : MonoBehaviour {

    public delegate void ChargerDealDamageEvent(int damage);
    public static event ChargerDealDamageEvent DecrementPlayerHealth;

    private Rigidbody2D _body2d;
    private float _direction;

    //public delegate void KnockPlayerUpEvent();
    //public static event KnockPlayerUpEvent KnockUp;

    [SerializeField]
    private int _damage;

    private void OnCollisionEnter2D(Collision2D collider) {

        if (collider.gameObject.tag == "Player") {
            if (DecrementPlayerHealth != null) {
                DecrementPlayerHealth(_damage);
            }
            //if (KnockUp != null) {
            //    KnockUp();
            //}

            // PLEASE CHECK IF THIS IS CORRECT ONCE THE FINAL SPRITE IS PUT INTO THE GAME
            _direction = transform.localScale.x > 0.0f ? -1.0f : 1.0f;

            _body2d = collider.gameObject.GetComponent<Rigidbody2D>();
            _body2d.AddForce(new Vector2(25000.0f * _direction, 20000.0f), ForceMode2D.Impulse);
        }
    }
}
