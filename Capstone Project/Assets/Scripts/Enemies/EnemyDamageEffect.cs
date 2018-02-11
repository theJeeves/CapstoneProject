using UnityEngine;
using System.Collections;

public class EnemyDamageEffect : MonoBehaviour {

    #region Constants
    private const float EFFECT_DURATION = 0.25f;
    private const float DIVIDEN = 5.0f;

    #endregion Constants

    #region Private Fields
    private SpriteRenderer _sprite;
    private SpriterDotNetUnity.ChildData _entity;

    #endregion Private Fields

    #region Private Initializers
    private void OnEnable() {
        // CHECK TO SEE IF THE ENEMY IS COMPOSED OF A SINGLE SPRITE OR WAS IMPORTED IN FROM SPRITER PRO
        if (gameObject.GetComponent<SpriteRenderer>() != null) {
            _sprite = GetComponent<SpriteRenderer>();
        }
        else if (gameObject.GetComponent<SpriterDotNetUnity.SpriterDotNetBehaviour>() != null) {
            _entity = GetComponent<SpriterDotNetUnity.SpriterDotNetBehaviour>().ChildData;
        }
    }

    #endregion Private Initializers

    #region Public Methods
    /// <summary>
    /// Play the damage effect animation.
    /// </summary>
    public void DamageEffect() {
        StartCoroutine(PlayEffect());
    }

    #endregion Public Methods

    #region Private Methods
    private IEnumerator PlayEffect() {
        if (_sprite != null) {

            float timer = 0.0f;
            while (timer < EFFECT_DURATION) {
                _sprite.color = Color.red;
                yield return new WaitForSeconds(EFFECT_DURATION / DIVIDEN);
                _sprite.color = Color.white;
                yield return new WaitForSeconds(EFFECT_DURATION / DIVIDEN);

                timer += EFFECT_DURATION / DIVIDEN;
            }
        }

        else if (_entity != null) {

            float timer = 0.0f;
            while (timer < EFFECT_DURATION) {

                foreach (GameObject sprite in _entity.Sprites) {
                    sprite.GetComponent<SpriteRenderer>().color = Color.red;
                }
                yield return new WaitForSeconds(EFFECT_DURATION / DIVIDEN);

                foreach (GameObject sprite in _entity.Sprites) {
                    sprite.GetComponent<SpriteRenderer>().color = Color.white;
                }
                yield return new WaitForSeconds(EFFECT_DURATION / DIVIDEN);

                timer += EFFECT_DURATION / DIVIDEN;
            }
        }
    }

    #endregion Private Methods
}
