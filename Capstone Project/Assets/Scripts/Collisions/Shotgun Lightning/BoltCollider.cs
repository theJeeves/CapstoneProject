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

    private Transform _start;
    private Transform _end;

    [SerializeField]
    private LayerMask enemies;

    // Use this for initialization
    private void Start() {
        _collider = GetComponent<CircleCollider2D>();
        _bolt = GetComponent<DigitalRuby.LightningBolt.LightningBoltScript>();

        _start = GameObject.FindGameObjectWithTag("Barrel").transform;
        _end = GameObject.FindGameObjectWithTag("Direction").transform;

        StartCoroutine(updateCollider());
    }

    private IEnumerator updateCollider() {

        //while (_previous != _collider.offset) {

        //    _previous = _collider.offset;
        //    _collider.offset = _bolt.EndPosition;

        //    yield return 0;
        //}

        //int skip = (1<<10);
        //RaycastHit2D hit = Physics2D.Raycast(_start.position, (_end.position - _start.position), 100.0f, enemies);
        //Debug.DrawLine(_start.position, (_end.position - _start.position) * 100.0f, Color.white);
        //if (hit.collider != null) {
        //    Debug.Log("hit that mother fucker!");
        //}
        yield return 0;
    }

    private void OnTriggerStay2D(Collider2D collider) {

        if (collider.gameObject.tag == "Enemy") {
            Debug.Log("damange enemy");
            if (DamageEnemy != null) {
                DamageEnemy(_damageAmount);
            }
        }
    }
}
