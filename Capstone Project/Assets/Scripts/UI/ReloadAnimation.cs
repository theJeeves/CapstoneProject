using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ReloadAnimation : MonoBehaviour {

    private Image _ammoImage;
    private float _timer = 0.0f;
    private bool _canAnimate = true;

    private void OnEnable() {
        Shotgun.StartReloadAnimation += Reload;
        Shotgun.EmptyClip += ZeroFillAmount;
        MachineGun.StartReloadAnimation += Reload;
        MachineGun.EmptyClip += ZeroFillAmount;

        _ammoImage = GetComponent<Image>();
    }

    private void OnDisable() {
        Shotgun.StartReloadAnimation -= Reload;
        Shotgun.EmptyClip -= ZeroFillAmount;
        MachineGun.StartReloadAnimation -= Reload;
        MachineGun.EmptyClip -= ZeroFillAmount;
    }

    private void Update() {

        if (!_canAnimate && _ammoImage.fillAmount < 1) {
            _ammoImage.fillAmount += 1.0f / _timer * Time.deltaTime;
        }
        else {
            _canAnimate = true;
            _timer = 0;
        }
    }

    private void Reload(float reloadTime) {
        
        if (_canAnimate) {
            _canAnimate = false;
            _timer = reloadTime;
        }
    }

    private void ZeroFillAmount() {
        _ammoImage.fillAmount = 0;
    }
}
