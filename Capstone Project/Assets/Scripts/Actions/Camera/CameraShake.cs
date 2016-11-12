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
            Camera.main.transform.position += new Vector3(Random.insideUnitCircle.x * _shakeAmount, Random.insideUnitCircle.y * _shakeAmount, 0);
            _timer -= _decreaseRate * Time.deltaTime;
            yield return 0;
        }

        /*
         * NEED TO FIX THIS. WHEN THIS LINE IS ENABLED, THE CAMERA MOVEMENT IS
         * WAY TO JARRING, BUT THE CAMERA DOES NOT GET SET BACK TO THE ORIGINAL POSITION.
         */
        //Camera.main.transform.position = _defaultCamPos;
    }
}
