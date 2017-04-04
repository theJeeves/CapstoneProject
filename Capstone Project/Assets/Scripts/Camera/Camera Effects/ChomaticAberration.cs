using UnityEngine;
using System.Collections;

public class ChomaticAberration : MonoBehaviour {

    [SerializeField]
    private float _effectIntensity = 0;
    [SerializeField]
    private float _effectDuration = 0.0f;

    private float _startTime = 0.0f;

    private UnityStandardAssets.ImageEffects.VignetteAndChromaticAberration _chroma;

    private void OnEnable() {
        _chroma = GetComponent<UnityStandardAssets.ImageEffects.VignetteAndChromaticAberration>();
    }

    public void PlayerDamaged(int ignore = 0) {
        StartCoroutine(PlayEffect());
    }

    private IEnumerator PlayEffect() {

        _startTime = Time.time;

        _chroma.chromaticAberration = _effectIntensity;

        while(_chroma.chromaticAberration > 0.0f) {
            _chroma.chromaticAberration = Mathf.SmoothStep(_chroma.chromaticAberration, 0.0f, (Time.time - _startTime) / _effectDuration);
            yield return 0;
        }
    }
}
