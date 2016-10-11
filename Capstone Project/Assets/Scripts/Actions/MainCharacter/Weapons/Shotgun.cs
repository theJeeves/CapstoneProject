using UnityEngine;
using System.Collections;

public class Shotgun : AbstractGun {

    protected override void OnButtonDown(Buttons button) {

        if (button == Buttons.Shoot && !_collisionState.OnSolidGround) {

            base.OnButtonDown(button);
        }
    }
}
