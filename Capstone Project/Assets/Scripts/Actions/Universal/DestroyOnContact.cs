using UnityEngine;
using System.Collections;

public class DestroyOnContact : MonoBehaviour {

    public delegate void DestroyOnContactEvent(int bulletDamage);
    public static event DestroyOnContactEvent DecrementHealth;

    [SerializeField]
    private int _bulletDamage;

    private void OnTriggerEnter2D(Collider2D collider) {

    if (collider.gameObject.tag == "Player" && DecrementHealth != null) {

            DecrementHealth(_bulletDamage);
            Destroy(gameObject);
        }
    }
}