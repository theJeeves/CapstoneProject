using UnityEngine;

public class CrystalBullet : AbstractBullet {

    [SerializeField]
    private SOEffects _SOEffectHandler;

    public override void Fire(Vector2 direction) {

        base.Fire(direction);

        // Have the crystals fire out from the MG at slightly off directions to give a more chaotic feel.
        _direction += new Vector2(Random.Range(_directionRange.Min, _directionRange.Max), Random.Range(_directionRange.Min, _directionRange.Max));

        GetComponent<Rigidbody2D>().velocity = _direction * _shotSpeed;
    }

    private void OnCollisionEnter2D(Collision2D otherGO) {

        if (otherGO?.gameObject.tag == Tags.EnemyTag)
        {
            otherGO.gameObject.GetComponentInParent<EnemyBasicBehaviors>().DecrementHealth(_damageAmount);
        }
        if (otherGO?.collider.gameObject.tag == Tags.SwarmerBatteryTag)
        {
            otherGO.gameObject.GetComponentInParent<SwarmPodSpawner>().DestroyPod();
        }

        if (otherGO?.gameObject.tag != Tags.PlayerTag)
        {
            _SOEffectHandler.PlayEffect(EffectEnums.CrystalImpact, transform.position, gameObject.transform.localEulerAngles.z);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D otherGO)
    {
        if (otherGO?.gameObject.tag == Tags.BlockTag)
        {
            _SOEffectHandler.PlayEffect(EffectEnums.CrystalImpact, transform.position, gameObject.transform.localEulerAngles.z);
            Destroy(gameObject);
        }
        else if (otherGO?.gameObject.tag == Tags.EnemyTag)
        {
            otherGO.gameObject.GetComponentInParent<EnemyBasicBehaviors>().DecrementHealth(_damageAmount);
            _SOEffectHandler.PlayEffect(EffectEnums.CrystalImpact, transform.position, gameObject.transform.localEulerAngles.z);
            Destroy(gameObject);
        }
    }
}
