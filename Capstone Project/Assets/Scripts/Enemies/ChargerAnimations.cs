using UnityEngine;
using System.Collections;
using SpriterDotNetUnity;
using System.Collections.Generic;
using System.Linq;

public class ChargerAnimations : MonoBehaviour {

    #region Constants
    private const float SCREEN_MIN = 0.1f;
    private const float SCREEN_MAX = 0.9f;
    private const float SHORT_DELAY = 0.2f;
    private const float LONG_DELAY = 1.0f;
    private const int DEFAULT_ANIMATION_INDEX = 1;

    #endregion Constants

    #region Private Fields
    private UnityAnimator _spriterAnimator;
    private List<string> _animationList;
    private bool _deployed = false;
    private Vector3 _GOpos;
    private int _currentIndex = 0;
    private int _previousIndex = -100;

    #endregion Private Fields

    #region Public Methods

    /// <summary>
    /// Play a specified animation by index.
    /// </summary>
    /// <param name="index"></param>
    public void Play(int index) {
        _currentIndex = index;
        _spriterAnimator.Play(_animationList[_currentIndex]);
    }

    #endregion Public Methods

    #region Private Methods
    private void Update() {

        // Check if the deployed animation has already played.
        if (!_deployed) {

            if (_spriterAnimator == null) {
                _spriterAnimator = GetComponent<SpriterDotNetBehaviour>().Animator;
                _animationList = _spriterAnimator.GetAnimations().ToList();
            }

            // Initially get the sniper's position relative to the camera view
            _GOpos = Camera.main.WorldToViewportPoint(transform.position);

            // KEEP CHECKING IF THE SNIPER IS WITHIN THE SCREEN TO ENSURE THE PLAYER SEES THE DEPLOY ANIMATION
            bool allowAction = _GOpos.x > SCREEN_MIN;
            allowAction &= _GOpos.x < SCREEN_MAX;
            allowAction &= _GOpos.y > SCREEN_MIN;
            allowAction &= _GOpos.y < SCREEN_MAX;

            if (allowAction) {
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

        yield return new WaitForSeconds(LONG_DELAY);
        GetComponentInChildren<ChargerLockOn>().enabled = true;
    }

    #endregion Private Methods
}
