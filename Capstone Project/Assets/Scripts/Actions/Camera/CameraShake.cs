using UnityEngine;
using System.Collections;

/*
 * Base class for all classes which need to shake the screen. It can be used for weapon firing,
 * damage taken, damage delt, etc. 
 */ 

public abstract class CameraShake : MonoBehaviour {

    [SerializeField]
    protected float _shakeTime;
    [SerializeField]
    protected float _shakeAmount;
    [SerializeField]
    protected float _decreaseRate;

    protected Vector3 _defaultCamPos;
    protected float _timer;

    protected void ShakeScreen() {

        _timer = _shakeTime;
        _defaultCamPos = Camera.main.transform.localPosition;
        StartCoroutine(Shake());
    }

    protected IEnumerator Shake() {

        while (_timer > 0.0f) {
            transform.localPosition += new Vector3(Random.insideUnitCircle.x * _shakeAmount, Random.insideUnitCircle.y * _shakeAmount, 0);
            _timer -= _decreaseRate * Time.deltaTime;
            yield return 0;
        }

        transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
    }
}
