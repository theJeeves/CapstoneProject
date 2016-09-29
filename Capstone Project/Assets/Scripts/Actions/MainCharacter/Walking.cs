using UnityEngine;
using System.Collections;

public class Walking : AbstractAction {

    [SerializeField]
    private float _xVelocity = 50.0f;

	
	// Update is called once per frame
	void Update () {

        if (_controller.GetButtonPressed(inputButtons[0]) ||
            _controller.GetButtonPressed(inputButtons[1])) {

            _body2d.velocity = new Vector2(_xVelocity * (float)_controller.Direction, _body2d.velocity.y);
        }
    }
}
