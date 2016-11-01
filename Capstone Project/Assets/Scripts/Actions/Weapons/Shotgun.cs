using UnityEngine;
using System.Collections;

public class Shotgun : AbstractGun {

    public static event AbstractGunEvent UpdateNumOfRounds;
    public static event AbstractGunEvent2 Fire;

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

            if (UpdateNumOfRounds != null && Fire != null) {
                UpdateNumOfRounds(--_numOfRounds);
                Fire();
            }

            _blast.Play();
            _canReload = false;

            base.OnButtonDown(button);
        }
    }

    protected override void Reload() {

        if (_canReload) {
            base.Reload();
            if (UpdateNumOfRounds != null) {
                UpdateNumOfRounds(_numOfRounds);
            }
        }
        _canReload = true;
    }
}
