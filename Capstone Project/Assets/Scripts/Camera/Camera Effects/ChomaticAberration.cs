using UnityEngine;
using System.Collections;

public class ChomaticAberration : MonoBehaviour {

    [SerializeField]
    private float _effectIntensity = 0;
    [SerializeField]
    private float _effectDuration = 0.0f;

    private float m_startTime = 0.0f;

    private UnityStandardAssets.ImageEffects.VignetteAndChromaticAberration _chroma;

    private void OnEnable() {
        _chroma = GetComponent<UnityStandardAssets.ImageEffects.VignetteAndChromaticAberration>();
    }

    public void PlayerDamaged(int ignore = 0) {
        StartCoroutine(PlayEffect());
    }

    private IEnumerator PlayEffect() {

        m_startTime = 0.0f;

        _chroma.chromaticAberration = _effectIntensity;

        while(_chroma.chromaticAberration > 0.0f) {
            _chroma.chromaticAberration = Mathf.SmoothStep(_chroma.chromaticAberration, 0.0f, TimeTools.TimeElapsed(ref m_startTime) / _effectDuration);
            yield return 0;
        }
    }
}
