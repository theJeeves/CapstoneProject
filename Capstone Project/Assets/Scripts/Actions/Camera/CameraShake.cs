using UnityEngine;
using System.Collections;

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
        _defaultCamPos = _camera.transform.position;
    }

    protected void ShakeScreen() {

        _timer = _shakeTime;
        StartCoroutine(Shake());
    }

    protected IEnumerator Shake() {

        while (_timer > 0.0f) {
            _camera.transform.position = new Vector3(Random.insideUnitCircle.x * _shakeAmount, Random.insideUnitCircle.y * _shakeAmount, _camera.transform.position.z);
            _timer -= _decreaseRate * Time.deltaTime;
            yield return 0;
        }

        _camera.transform.position = _defaultCamPos;
    }
}
