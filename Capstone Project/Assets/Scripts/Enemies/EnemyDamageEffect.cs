using UnityEngine;
using System.Collections;

public class EnemyDamageEffect : MonoBehaviour {

    private SpriteRenderer _sprite;
    private Spriter2UnityDX.EntityRenderer _entity;

    private void OnEnable() {
        _sprite = GetComponent<SpriteRenderer>();
        _entity = GetComponent<Spriter2UnityDX.EntityRenderer>();
    }


    public void DamageEffect() {
        StartCoroutine(PlayEffect());
    }

    private IEnumerator PlayEffect() {
        if (_sprite != null) {
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

        else if (_entity != null) {
            _entity.Color = Color.red;
            yield return new WaitForSeconds(0.1f);
            _entity.Color = Color.white;
            yield return new WaitForSeconds(0.1f);
            _entity.Color = Color.red;
            yield return new WaitForSeconds(0.1f);
            _entity.Color = Color.white;
            yield return new WaitForSeconds(0.1f);
            _entity.Color = Color.red;
            yield return new WaitForSeconds(0.1f);
            _entity.Color = Color.white;
            yield return new WaitForSeconds(0.1f);
            _entity.Color = Color.red;
            yield return new WaitForSeconds(0.1f);
            _entity.Color = Color.white;
        }
    }
}
