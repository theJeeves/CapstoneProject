using UnityEngine;
using System.Collections;

public class CrystalBullet : AbstractBullet {

    [SerializeField]
    private SOEffects _SOEffectHandler;

    protected override void Start() {
        base.Start();

        // Have the crystals fire out from the MG at slightly off directions to give a more chaotic feel.
        _direction += new Vector2(Random.Range(_directionRange.Min, _directionRange.Max), Random.Range(_directionRange.Min, _directionRange.Max));

        GetComponent<Rigidbody2D>().velocity = _direction * _shotSpeed;
    }

    private void OnCollisionEnter2D(Collision2D otherGO) {

        if (otherGO.gameObject.tag == "Enemy") {
            otherGO.gameObject.GetComponentInParent<EnemyHealth>().DecrementHealth(_damageAmount);
        }

        if (otherGO.gameObject.tag != "Player" && otherGO != null) {
            _SOEffectHandler.PlayEffect(EffectEnums.CrystalImpact, transform.position, gameObject.transform.localEulerAngles.z);
            Destroy(gameObject);
        }
    }
}
