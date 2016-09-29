using UnityEngine;
using System.Collections;

public class FacingDirection : AbstractAction {

	
	// Update is called once per frame
	void Update () {

        if (_controller.GetButtonPressed(inputButtons[0])) {
            _controller.Direction = Facing.Right;
        }
        else if(_controller.GetButtonPressed(inputButtons[1])) {
            _controller.Direction = Facing.Left;
        }
	}
}
