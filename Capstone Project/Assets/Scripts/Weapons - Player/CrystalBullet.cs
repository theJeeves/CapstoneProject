using UnityEngine;
using System.Collections;

public class CrystalBullet : AbstractBullet {

    [SerializeField]
    private SOEffects _SOEffect;

    private ControllableObject _controller;

    private void OnEnable() {
        _controller = GameObject.FindGameObjectWithTag("Player").GetComponent<ControllableObject>();
    }

    protected override void Start() {
        base.Start();

        // Have the crystals fire out from the MG at slightly off directions to give a more chaotic feel.
        _direction += new Vector3(Random.Range(_directionRange.Min, _directionRange.Max), Random.Range(_directionRange.Min, _directionRange.Max), 0);

        GetComponent<Rigidbody2D>().velocity = _direction * _shotSpeed;
    }

    private void OnCollisionEnter2D(Collision2D otherGO) {

        if (otherGO.gameObject.tag == "Enemy") {
            otherGO.gameObject.GetComponentInParent<EnemyHealth>().DecrementHealth(_damageAmount);
        }

        if (otherGO != null) {
            _SOEffect.PlayEffect(EffectEnum.MGImpact, transform.position, gameObject.transform.localEulerAngles.z);
            Destroy(gameObject);
        }
    }
}
