using UnityEngine;
using System.Collections;

public class AcidBallBehavior : MonoBehaviour {

    [SerializeField]
    private SOEffects _SOEffect;

    private AudioSource _audioSource;

    private void OnEnable() {
        GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-75.0f, 75.0f), Random.Range(75.0f, 250.0f)), ForceMode2D.Impulse);
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D otherGO) {

        Splatter();
        
        if (otherGO.collider.tag == "Player") {
            otherGO.gameObject.GetComponent<PlayerHealth>().DecrementPlayerHealth(2, 5.0f, DamageEnum.Acid);
        }
    }

    public void Splatter() {
        _SOEffect.PlayEffect(_audioSource, transform.position, -90.0f);
        Destroy(gameObject);
    }
}
