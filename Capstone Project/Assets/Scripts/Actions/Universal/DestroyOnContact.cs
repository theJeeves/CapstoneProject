using UnityEngine;
using System.Collections;

public class DestroyOnContact : MonoBehaviour {

    public delegate void DestroyOnContactEvent(float bulletDamage);
    public static event DestroyOnContactEvent DecrementHealth;

    [SerializeField]
    private GameObject _otherObject;

    [SerializeField]
    private float _bulletDamage;

    private void OnTriggerEnter2D(Collider2D collider) {

        if (collider.gameObject.tag == "Player") {
            if(DecrementHealth != null) {
                DecrementHealth(_bulletDamage);
            }
            Destroy(gameObject);
        }
    }
}