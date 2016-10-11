using UnityEngine;
using System.Collections;

public class MachineGun : AbstractGun {

    protected override void OnEnable() {
        ControllableObject.OnButton += OnButton;
    }

    protected override void OnDisable() {
        ControllableObject.OnButton -= OnButton;
    }

    private void OnButton(Buttons button) {
        if (button == Buttons.Shoot && !_collisionState.OnSolidGround) {

            base.OnButtonDown(button);
        }
    }
}
