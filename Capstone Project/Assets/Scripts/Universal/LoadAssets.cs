using UnityEngine;
using System.Collections;

public class LoadAssets : MonoBehaviour {

    [SerializeField]
    private SOEffects _SOEffectHandler;

    private void Awake() {
        _SOEffectHandler.LoadEffects();
    }
}
