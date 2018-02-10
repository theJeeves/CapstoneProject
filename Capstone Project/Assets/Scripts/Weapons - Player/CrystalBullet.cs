using UnityEngine;
using System.Collections;

public class CrystalBullet : AbstractBullet {

    [SerializeField]
    private SOEffects _SOEffectHandler;

    public override void Fire(Vector2 direction) {

        base.Fire(direction);

        // Have the crystals fire out from the MG at slightly off directions to give a more chaotic feel.
        m_Direction += new Vector2(Random.Range(m_DirectionRange.Min, m_DirectionRange.Max), Random.Range(m_DirectionRange.Min, m_DirectionRange.Max));

        GetComponent<Rigidbody2D>().velocity = m_Direction * m_ShotSpeed;
    }

    private void OnCollisionEnter2D(Collision2D otherGO) {

        if (otherGO.gameObject.tag == "Enemy") {
            otherGO.gameObject.GetComponentInParent<EnemyBasicBehaviors>().DecrementHealth(m_DamageAmount);
        }
        if (otherGO.collider.gameObject.tag == "SwarmerPodBattery") {
            otherGO.gameObject.GetComponentInParent<SwarmPodSpawner>().DestroyPod();
        }

        if (otherGO.gameObject.tag != "Player" && otherGO != null) {
            _SOEffectHandler.PlayEffect(EffectEnums.CrystalImpact, transform.position, gameObject.transform.localEulerAngles.z);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D otherGO) {

        if (otherGO.gameObject.tag == "Block") {
            _SOEffectHandler.PlayEffect(EffectEnums.CrystalImpact, transform.position, gameObject.transform.localEulerAngles.z);
            Destroy(gameObject);
        }
        else if (otherGO.gameObject.tag == "Enemy") {
            otherGO.gameObject.GetComponentInParent<EnemyBasicBehaviors>().DecrementHealth(m_DamageAmount);
            _SOEffectHandler.PlayEffect(EffectEnums.CrystalImpact, transform.position, gameObject.transform.localEulerAngles.z);
            Destroy(gameObject);
        }
    }
}
