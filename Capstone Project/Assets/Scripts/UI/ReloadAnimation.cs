using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ReloadAnimation : MonoBehaviour {

    private Image _ammoImage;
    private float _timer = 0.0f;
    private bool _canAnimate = true;

    private void OnEnable() {
        Shotgun.StartReloadAnimation += Reload;

        _ammoImage = GetComponent<Image>();
    }

    private void OnDisable() {
        Shotgun.StartReloadAnimation -= Reload;
    }

    private void Update() {

        if (_canAnimate && _ammoImage.fillAmount < 1) {
            _ammoImage.fillAmount += 1.0f / _timer * Time.deltaTime;
        }
        else {
            _canAnimate = true;
        }
    }

    private void Reload() {
        
        if (_canAnimate) {
            _canAnimate = false;
                        
        }
    }

    private void ZeroFillAmount() {
        _ammoImage.fillAmount = 0;
    }
}
