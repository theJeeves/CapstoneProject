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

    private ControllableObject _controller;

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
        if (_controller == null) {
            _controller = GameObject.FindGameObjectWithTag("Player").GetComponent<ControllableObject>();
        }

        //_timer = _shakeTime;
        StartCoroutine(Shake());
    }

    protected IEnumerator Shake() {

        float currentShake = _shakeAmount;

        while (currentShake != 0.0f) {

            transform.localPosition = _directions[_controller.CurrentKey] * currentShake;
            currentShake = Mathf.MoveTowards(currentShake, 0.0f, _decreaseRate);
            yield return new WaitForSeconds(_shakeDelay);
            transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        }
        yield return 0;
    }

    private void AssignDirections(int angle, float x, float y, float z = 0.0f) {
        _directions[angle] = new Vector3(x, y, z);
    }
}
