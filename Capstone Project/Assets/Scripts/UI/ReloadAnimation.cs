using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public abstract class ReloadAnimation : MonoBehaviour {

    private Image _ammoImage;
    private float _timer = 0.0f;
    //private bool _canAnimate = true;

    protected virtual void Awake() {
        _ammoImage = GetComponent<Image>();
    }

    protected virtual void OnEnable() { 
    }

    protected virtual void OnDisable() {
        if (_ammoImage.fillAmount < 1.0f) {
            _ammoImage.fillAmount = 1.0f;
        }
    }

    protected virtual void Update() {

        if (_ammoImage.fillAmount < 1) {
            _ammoImage.fillAmount += 1.0f / _timer * Time.deltaTime;
        }
    }

    protected virtual void Reload(float reloadTime) {

        _ammoImage.fillAmount = 0;
        _timer = reloadTime;
    }

    protected virtual void ZeroFillAmount() {
    }
}
