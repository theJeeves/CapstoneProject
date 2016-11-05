using UnityEngine;
using System.Collections;

public class BoltCollider : MonoBehaviour {

    public delegate void BoltColliderEvent(float damageAmount);
    public static event BoltColliderEvent DamageEnemy;

    [SerializeField]
    private float _damageAmount;

    private CircleCollider2D _collider;
    private Vector2 _previous = Vector2.right;
    private DigitalRuby.LightningBolt.LightningBoltScript _bolt;

    // Use this for initialization
    private void Start() {
        _collider = GetComponent<CircleCollider2D>();
        _bolt = GetComponent<DigitalRuby.LightningBolt.LightningBoltScript>();

        StartCoroutine(updateCollider());
    }

    private IEnumerator updateCollider() {

        while (_previous != _collider.offset) {

            _previous = _collider.offset;
            _collider.offset = _bolt.EndPosition;

            yield return 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider) {

        if (collider.gameObject.tag == "Enemy") {
            Debug.Log("damange enemy");
            if (DamageEnemy != null) {
                DamageEnemy(_damageAmount);
            }
        }
    }
}
