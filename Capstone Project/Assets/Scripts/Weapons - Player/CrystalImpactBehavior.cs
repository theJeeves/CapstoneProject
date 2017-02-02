using UnityEngine;
using System.Collections;

public class CrystalImpactBehavior : MonoBehaviour {

    [SerializeField]
    private SOEffects _SOEffect;

    private void OnEnable() {
        _SOEffect.PlayAudioEffect(GetComponent<AudioSource>());
    }
}
