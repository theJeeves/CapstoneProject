using UnityEngine;
using System.Collections;

public class Shotgun : AbstractGun {

    public static event AbstractGunEvent UpdateNumOfRounds;

    private ParticleSystem _blast;
    private bool _canReload;

    protected override void OnEnable() {
        base.OnEnable();

        _blast = GetComponentInChildren<ParticleSystem>();
        _canReload = true;
        if (UpdateNumOfRounds != null) {
            UpdateNumOfRounds(_numOfRounds);
        }
    }

    private void Start() {
        _numOfRounds = _clipSize;
        _canReload = true;
        if (UpdateNumOfRounds != null) {
            UpdateNumOfRounds(_numOfRounds);
        }
    }

    protected override void OnButtonDown(Buttons button) {

        if (button == Buttons.Shoot && _numOfRounds > 0) {

            if (UpdateNumOfRounds != null) {
                UpdateNumOfRounds(--_numOfRounds);
            }

            _blast.Play();
            _canReload = false;

            //if (_numOfRounds == _clipSize - 1) {
            //    StartCoroutine(Reload());
            //}

            base.OnButtonDown(button);
        }
    }

    protected override void Reload() {

        if (_canReload) {
            base.Reload();
            UpdateNumOfRounds(_numOfRounds);
        }
        _canReload = true;
    }

    //private IEnumerator Reload() {
    //    while (_numOfRounds < _clipSize) {
    //        yield return new WaitForSeconds(_coolDownTime);

    //        ++_numOfRounds;
    //        if (UpdateNumOfRounds != null && isActiveAndEnabled) {
    //            UpdateNumOfRounds(_numOfRounds);
    //        }
    //    }
    //}
}
