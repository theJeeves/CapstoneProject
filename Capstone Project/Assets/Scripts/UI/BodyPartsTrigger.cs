using UnityEngine;
using System.Collections;

public class BodyPartsTrigger : MonoBehaviour {

    private StartWindowEffects _startEffect;

    private void OnEnable() {
        _startEffect = GetComponent<StartWindowEffects>();
    }

    private void OnTriggerEnter2D(Collider2D otherGO) {

        if (otherGO.tag == "Player") {
            Destroy(otherGO.gameObject);
            _startEffect.SetStart();
        }
    }
}
