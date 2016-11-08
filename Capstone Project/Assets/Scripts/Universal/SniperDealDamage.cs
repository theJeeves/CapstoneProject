using UnityEngine;
using System.Collections;

public class SniperDealDamage : AbstractEnemyDealDamage {


    protected override void OnTriggerEnter2D(Collider2D collider) {
        base.OnTriggerEnter2D(collider);

        if (collider.gameObject.tag == "Player") {
            Destroy(gameObject);
        }
    }
}