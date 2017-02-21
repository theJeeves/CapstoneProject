using UnityEngine;
using System.Collections;
using SpriterDotNetUnity;
using System.Collections.Generic;
using System.Linq;

public class SniperAnimations : MonoBehaviour {

    private UnityAnimator _spriterAnimator;
    private List<string> _animationList;
    private bool _deployed = false;
    private Vector3 _GOpos;

    private int _currentIndex = 0;
    private int _previousIndex = -100;
	
	private void Update () {

        // Check if the deployed animation has already played.
        if (!_deployed) {

            if (_spriterAnimator == null) {
                _spriterAnimator = GetComponent<SpriterDotNetBehaviour>().Animator;
                _animationList = _spriterAnimator.GetAnimations().ToList();
            }

            // Initially get the sniper's position relative to the camera view
            _GOpos = Camera.main.WorldToViewportPoint(transform.position);

            // KEEP CHECKING IF THE SNIPER IS WITHIN THE SCREEN TO ENSURE THE PLAYER SEES THE DEPLOY ANIMATION
            if (_GOpos.x > 0.1f && _GOpos.x < 0.9f && _GOpos.y > 0.1 && _GOpos.y < 0.9F) {
                StartCoroutine(DeployDelay());
                _deployed = true;
            }
        }
        else {
            if (_currentIndex != _previousIndex) {
                Play(_currentIndex);
                _previousIndex = _currentIndex;
            }
        }
    }

    public void Play(int index) {
        _currentIndex = index;
        _spriterAnimator.Play(_animationList[_currentIndex]);
    }

    private IEnumerator DeployDelay() {

        yield return new WaitForSeconds(0.2f);
        _currentIndex = 1;
        _previousIndex = 1;
        Play(_currentIndex);

        // Make the sniper enemy rise from its inital position
        int counter = 0;
        while (true) {
            transform.position += new Vector3(0.0f, 1.0f, 0.0f);
            yield return 0;
            if (++counter > 13) {
                break;
            }
        }

        yield return new WaitForSeconds(1.0f);
        GetComponentInChildren<SniperLockOn>().enabled = true;
        GetComponentInChildren<LaserSights>().enabled = true;
    }
}
