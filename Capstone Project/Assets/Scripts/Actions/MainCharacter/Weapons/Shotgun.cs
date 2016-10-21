using UnityEngine;
using System.Collections;

public class Shotgun : AbstractGun {

    [SerializeField]
    private int _clipSize;
    [SerializeField]
    private int _numOfRounds;
    [SerializeField]
    private float _coolDownTime;

    private void Start() {
        _numOfRounds = _clipSize;
    }

    protected override void OnButtonDown(Buttons button) {

        //if (button == Buttons.Jump && !_collisionState.OnSolidGround) {

        //    base.OnButtonDown(button);
        //}
        if (button == Buttons.Shoot && _numOfRounds > 0) {

            --_numOfRounds;

            if (_numOfRounds == _clipSize - 1) {
                StartCoroutine(Reload());
            }

            base.OnButtonDown(button);
        }
    }

    private IEnumerator Reload() {
        while (_numOfRounds < _clipSize) {
            yield return new WaitForSeconds(_coolDownTime);
            ++_numOfRounds;
        }
    }
}
