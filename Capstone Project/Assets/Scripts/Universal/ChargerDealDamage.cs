using UnityEngine;
using System.Collections;

public class ChargerDealDamage : MonoBehaviour {

    public delegate void ChargerDealDamageEvent(int damage);
    public static event ChargerDealDamageEvent DecrementPlayerHealth;

    private Rigidbody2D _body2d;
    private float _direction;

    [SerializeField]
    private int _damage;

    private void OnCollisionEnter2D(Collision2D otherGO) {

        if (otherGO.gameObject.tag == "Player") {

            otherGO.gameObject.GetComponent<PlayerHealth>().DecrementPlayerHealth(_damage);

            // PLEASE CHECK IF THIS IS CORRECT ONCE THE FINAL SPRITE IS PUT INTO THE GAME
            _direction = transform.localScale.x > 0.0f ? -1.0f : 1.0f;

            // Hit the player back!
            otherGO.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(_direction * 100.0f, 100.0f);

            // Stop the charger enemy from moving after the player is hit.
            GetComponent<Rigidbody2D>().velocity = new Vector3(0.0f, 0.0f, 0.0f);
        }
    }
}
