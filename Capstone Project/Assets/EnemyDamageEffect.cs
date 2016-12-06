using UnityEngine;
using System.Collections;

public class EnemyDamageEffect : MonoBehaviour {

    private SpriteRenderer _sprite;

    private void OnEnable() {
        EnemyHealth.Damaged += DamageEffect;
        _sprite = GetComponent<SpriteRenderer>();
    }

    private void OnDisable() {
        EnemyHealth.Damaged -= DamageEffect;
    }

    private void DamageEffect(GameObject thisEnemy) {
        if (thisEnemy.GetComponentInChildren<SpriteRenderer>() == _sprite) {
            StartCoroutine(PlayEffect());
        }
    }

    private IEnumerator PlayEffect() {
        _sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        _sprite.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        _sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        _sprite.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        _sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        _sprite.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        _sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        _sprite.color = Color.white;
    }
}
