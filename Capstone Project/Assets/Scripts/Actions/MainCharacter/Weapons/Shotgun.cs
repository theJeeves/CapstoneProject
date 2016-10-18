using UnityEngine;
using System.Collections;

public class Shotgun : AbstractGun {

    protected override void OnButtonDown(Buttons button) {

        if (_version == 1) {
            if (button == Buttons.Jump && !_collisionState.OnSolidGround) {

                base.OnButtonDown(button);
            }
        }
        else if (_version == 2) {
            if (button == Buttons.Shoot && !_collisionState.OnSolidGround) {

                base.OnButtonDown(button);
            }
        }
    }
}
