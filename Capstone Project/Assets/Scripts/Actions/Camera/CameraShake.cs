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

    protected GameObject _camera;
    protected Vector3 _defaultCamPos;
    protected float _timer;

	protected void Awake() {
        _camera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    protected void ShakeScreen() {

        _timer = _shakeTime;
        _defaultCamPos = _camera.transform.localPosition;
        StartCoroutine(Shake());
    }

    protected IEnumerator Shake() {

        while (_timer > 0.0f) {
            _camera.transform.localPosition += new Vector3(Random.insideUnitCircle.x * _shakeAmount, Random.insideUnitCircle.y * _shakeAmount, 0);
            _timer -= _decreaseRate * Time.deltaTime;
            yield return 0;
        }

        _camera.transform.localPosition = _defaultCamPos;
    }
}
