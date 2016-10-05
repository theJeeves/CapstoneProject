using UnityEngine;
using System.Collections;

public class GunTest2 : AbstractPlayerActions {

    [SerializeField]
    private float _blowBack;

    protected override void OnButtonDown(Buttons button) {
        if (button == Buttons.Shoot2 && !_collisionState.OnSolidGround) {
            _body2d.velocity = new Vector2(0.5f *_blowBack * (float)_controller.Direction, _blowBack);
        }
    }
}
