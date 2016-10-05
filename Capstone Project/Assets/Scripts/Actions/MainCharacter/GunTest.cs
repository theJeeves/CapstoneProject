using UnityEngine;
using System.Collections;

public class GunTest : AbstractPlayerActions {

    [SerializeField]
    private float _blowBack;


	protected override void OnEnable() {
        ControllableObject.OnButton += OnButton;
    }

    protected override void OnDisable() {
        ControllableObject.OnButton -= OnButton;
    }

    private void OnButton(Buttons button) {

        if (button == Buttons.Shoot && !_collisionState.OnSolidGround) {
            _body2d.velocity = new Vector2(_body2d.velocity.x, _blowBack);
        }
    }
}
