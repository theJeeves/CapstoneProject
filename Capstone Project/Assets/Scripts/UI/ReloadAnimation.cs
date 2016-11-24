using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public abstract class ReloadAnimation : MonoBehaviour {

    protected Image _ammoImage;
    private float _timer = 0.0f;
    private bool _canAnimate;

    protected virtual void Awake() {
        _ammoImage = GetComponent<Image>();
        _canAnimate = false;
    }

    protected virtual void OnDisable() {

        if (_ammoImage.fillAmount < 1.0f) {
            _ammoImage.fillAmount = 1.0f;
        }
    }

    protected virtual void Update() {

        if (_canAnimate && _ammoImage.fillAmount < 1) {
            _ammoImage.fillAmount += 1.0f / _timer * Time.deltaTime;
        }
        else if (_canAnimate && _ammoImage.fillAmount >= 1) {
            _canAnimate = false;
        }
    }

    protected virtual void Reload(float reloadTime) {

        _ammoImage.fillAmount = 0;
        _timer = reloadTime;
        _canAnimate = true;
    }

    protected virtual void ZeroFillAmount() {
        _ammoImage.fillAmount = 0;
    }
}
