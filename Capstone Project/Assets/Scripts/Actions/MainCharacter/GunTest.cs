using UnityEngine;
using System.Collections;

public class GunTest : AbstractPlayerActions {

    [SerializeField]
    private float _blowBack;


	private void OnEnable() {
        ControllableObject.OnButton += OnButtonDown;
    }

    private void OnDisable() {
        ControllableObject.OnButton -= OnButtonDown;
    }

    private void OnButtonDown(Buttons button) {
        if (button == Buttons.Shoot && !_collisionState.OnSolidGround) {
            _body2d.velocity = new Vector2(_body2d.velocity.x, _blowBack);
        }
    }
}
