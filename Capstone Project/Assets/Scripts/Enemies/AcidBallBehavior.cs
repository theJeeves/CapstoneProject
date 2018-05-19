using UnityEngine;

public class AcidBallBehavior : MonoBehaviour {

    #region Constants
    private const float MIN_X_RANGE = -75.0f;
    private const float MAX_X_RANGE = 75.0f;
    private const float MIN_Y_RANGE = 75.0f;
    private const float MAX_Y_RANGE = 250.0f;
    private const int DAMAGE_AMOUNT = 10;
    private const float DAMAGE_DURATION = 3.0f;
    private const float EFFECT_ANGLE = 50.0f;

    #endregion Constants

    #region Private Fields
    [SerializeField]
    private SOEffects _SOEffectHandler;

    #endregion Private Fields

    private void OnEnable() {
        GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(MIN_X_RANGE, MAX_X_RANGE), Random.Range(MIN_Y_RANGE, MAX_Y_RANGE)), ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D otherGO) {

        if (otherGO.gameObject.tag != Tags.EnemyTag) { Splatter(); }
        
        if (otherGO.collider.tag == Tags.PlayerTag) {
            otherGO.gameObject.GetComponent<PlayerHealth>().DecrementPlayerHealth(DAMAGE_AMOUNT, DAMAGE_DURATION, DamageEnum.Acid);
        }
    }

    public void Splatter() {
        _SOEffectHandler.PlayEffect(EffectEnums.AcidBallSplatter, transform.position, EFFECT_ANGLE);
        Destroy(gameObject);
    }
}
