using UnityEngine;
using System.Collections;

public class EnemyDamageEffect : MonoBehaviour {

    private float _effectDuration = 0.7f;

    private SpriteRenderer _sprite;
    private SpriterDotNetUnity.ChildData _entity;

    private void OnEnable() {
        // CHECK TO SEE IF THE ENEMY IS COMPOSED OF A SINGLE SPRITE OR WAS IMPORTED IN FROM SPRITER PRO
        if (gameObject.GetComponent<SpriteRenderer>() != null) {
            _sprite = GetComponent<SpriteRenderer>();
        }
        else if (gameObject.GetComponent<SpriterDotNetUnity.SpriterDotNetBehaviour>() != null) {
            _entity = GetComponent<SpriterDotNetUnity.SpriterDotNetBehaviour>().ChildData;
        }
    }


    public void DamageEffect() {
        StartCoroutine(PlayEffect());
    }

    private IEnumerator PlayEffect() {
        if (_sprite != null) {

            float timer = 0.0f;
            while (timer < _effectDuration) {
                _sprite.color = Color.red;
                yield return new WaitForSeconds(_effectDuration / 7.0f);
                _sprite.color = Color.white;
                yield return new WaitForSeconds(_effectDuration / 7.0f);

                timer += _effectDuration / 7.0f;
            }
        }

        else if (_entity != null) {

            float timer = 0.0f;
            while (timer < _effectDuration) {

                foreach (GameObject sprite in _entity.Sprites) {
                    sprite.GetComponent<SpriteRenderer>().color = Color.red;
                }
                yield return new WaitForSeconds(_effectDuration / 7.0f);

                foreach (GameObject sprite in _entity.Sprites) {
                    sprite.GetComponent<SpriteRenderer>().color = Color.white;
                }
                yield return new WaitForSeconds(_effectDuration / 7.0f);

                timer += _effectDuration / 7.0f;
            }
        }
    }
}
