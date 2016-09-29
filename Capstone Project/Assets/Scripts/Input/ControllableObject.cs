using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ButtonState {

    private bool _isPressed = false;
    public bool IsPressed {
        get { return _isPressed; }
        set { _isPressed = value; }
    }

    private float _pressTime = 0.0f;
    public float PressTime {
        get { return _pressTime; }
        set { _pressTime = value; }
    }
}

public enum Facing {
    Right = 1,
    Left = -1
}

public class ControllableObject : MonoBehaviour {

    [SerializeField]
    private Facing _facingDirection = Facing.Right;
    public Facing Direction {
        get { return _facingDirection; }
        set { _facingDirection = value; }
    }

    private Dictionary<Buttons, ButtonState> buttonStates = new Dictionary<Buttons, ButtonState>();

    public void SetButtonState(Buttons button, bool isPressed) {

        if (!buttonStates.ContainsKey(button)) {
            buttonStates.Add(button, new ButtonState());
        }

        if (buttonStates[button].IsPressed && !isPressed) {
            buttonStates[button].PressTime = 0.0f;
        }
        else if (buttonStates[button].IsPressed && isPressed) {
            buttonStates[button].PressTime += Time.deltaTime;
        }

        buttonStates[button].IsPressed = isPressed;
    }

    public bool GetButtonPressed(Buttons button) {

        if (buttonStates.ContainsKey(button)) {
            return buttonStates[button].IsPressed;
        }
        else {
            return false;
        }
    }

    public float GetButtonPressTime(Buttons button) {
        if (buttonStates.ContainsKey(button)) {
            return buttonStates[button].PressTime;
        }
        else {
            return 0.0f;
        }
    }
}
