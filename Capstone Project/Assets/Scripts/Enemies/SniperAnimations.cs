using UnityEngine;
using System.Collections;
using SpriterDotNetUnity;
using System.Collections.Generic;
using System.Linq;

public class SniperAnimations : MonoBehaviour {

    #region Constants
    private const int DEFAULT_ANIMATION_INDEX = 1;
    private const int DEPLOYMENT_DURATION = 13;
    private const float SHORT_DELAY = 0.2f;
    private const float LONG_DELAY = 1.0f;

    #endregion Constants

    private UnityAnimator _spriterAnimator;
    private List<string> _animationList;
    private bool _deployed = false;
    private Vector3 _GOpos;

    private int _currentIndex = 0;
    private int _previousIndex = int.MinValue;

    #region Public Methods
    /// <summary>
    /// Play a specified animation by index.
    /// </summary>
    /// <param name="index"></param>
    public void Play(int index) {
        _currentIndex = index;
        _spriterAnimator.Play(_animationList[_currentIndex]);

        if (index == 1) {
            GetComponent<AudioSource>().Play();
        }
    }

    #endregion Public Methods

    #region Private Methods
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
            if (_GOpos.x > 0.1f && _GOpos.x < 1.0f && _GOpos.y > 0.1 && _GOpos.y < 1.0F) {
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

    private IEnumerator DeployDelay() {

        yield return new WaitForSeconds(SHORT_DELAY);
        _currentIndex = DEFAULT_ANIMATION_INDEX;
        _previousIndex = DEFAULT_ANIMATION_INDEX;
        Play(_currentIndex);

        // Make the sniper enemy rise from its inital position
        int counter = 0;
        while (true) {
            transform.position += Vector3.up;
            yield return 0;
            if (++counter > DEPLOYMENT_DURATION) {
                break;
            }
        }

        yield return new WaitForSeconds(LONG_DELAY);
        GetComponentInChildren<SniperLockOn>().enabled = true;
        GetComponentInChildren<LaserSights>().enabled = true;
    }

    #endregion Private Methods
}
