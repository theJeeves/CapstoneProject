using UnityEngine;
using System.Collections;

/*
 * Base class for all classes which need to shake the screen. It can be used for weapon firing,
 * damage taken, damage delt, etc. 
 */ 

public abstract class CameraShake : MonoBehaviour {

    [SerializeField]
    protected float _shakeAmount;                   // The amount the screen should phsyically shake
    [SerializeField]
    protected float _decreaseRate;                  // The rate at which the screen should go back to baseline
    [SerializeField]
    protected float _shakeDelay;                    // How quickly should the screen shake

    private ControllableObject _player;
    protected Vector3 _defaultCamPos = new Vector3(0.0f, 0.0f, 0.0f);   // The position the camera should always go back to
    //protected float _timer;

    private Vector3[] _directions = new Vector3[8];     // All possible angles which can be used by the player

    protected virtual void OnEnable() {

        // Define all the possible angles based.
        AssignDirections(0, 1.0f, 0.0f);
        AssignDirections(1, 0.7f, 0.7f);
        AssignDirections(2, 0.0f, 1.0f);
        AssignDirections(3, -0.7f, 0.7f);
        AssignDirections(4, -1.0f, 0.0f);
        AssignDirections(5, -0.7f, -0.7f);
        AssignDirections(6, 0.0f, -1.0f);
        AssignDirections(7, 0.7f, -0.7f);
    }

    protected void ShakeScreen() {

        // Find the player only once. This is done here because we do not need to search for it at the beginning.
        // Only when the player first shoots do we need this reference.
        if (_player == null) {
            _player = GameObject.FindGameObjectWithTag("Player").GetComponent<ControllableObject>();
        }

        //_timer = _shakeTime;
        StartCoroutine(Shake());
    }

    protected IEnumerator Shake() {

        float currentShake = _shakeAmount;

        while (currentShake != 0.0f) {

            transform.localPosition = _directions[_player.CurrentKey] * currentShake;
            currentShake = Mathf.MoveTowards(currentShake, 0.0f, _decreaseRate);
            yield return new WaitForSeconds(_shakeDelay);
            transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        }
        yield return 0;

        //while (_timer > 0.0f) {
        //    transform.localPosition += new Vector3(Random.insideUnitCircle.x * _shakeAmount, Random.insideUnitCircle.y * _shakeAmount, 0);
        //    _timer -= _decreaseRate * Time.deltaTime;
        //    yield return 0;
        //}

        //Vector3 direction = new Vector3(0.0f, 0.0f, 0.0f);
        //Debug.Log(_player.AimDirection);
        //switch (_player.AimDirection) {
        //    case 0:
        //        direction = new Vector3(-_shakeAmount, 0.0f, 0.0f);
        //        break;
        //    case 45:
        //        direction = new Vector3(-_shakeAmount, -_shakeAmount, 0.0f);
        //        break;
        //    case 90:
        //        direction = new Vector3(0.0f, -_shakeAmount, 0.0f);
        //        break;
        //    case 135:
        //        direction = new Vector3(_shakeAmount, -_shakeAmount, 0.0f);
        //        break;
        //    case 180:
        //        direction = new Vector3(_shakeAmount, 0.0f, 0.0f);
        //        break;
        //}

        //transform.localPosition += direction;

        //yield return new WaitForSeconds(0.05f);

        //transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
    }

    private void AssignDirections(int angle, float x, float y, float z = 0.0f) {
        _directions[angle] = new Vector3(x, y, z);
    }
}
