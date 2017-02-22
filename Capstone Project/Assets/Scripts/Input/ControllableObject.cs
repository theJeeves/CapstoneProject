using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This script is used to maintain a record of all the buttons and their state for Action scripts to query against.
/// </summary>
/// 

// This helps other scripts determine which way the player should be facing/moving.
// This is here primarily because each AbstractAction script will call upon this script,
// meaning all other action script will have auto access to this variable for processing.
public enum Facing {
    Right = 1,
    Left = -1
}

// Class which keeps track of whether or not a button has been pressed and
// for the durraction it has been pressed.
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


// This class allows for action scripts to determine which direction the player is aiming.
// Primarily used for guns and traversal.
public class AimDirection {

    private bool _up;
    public bool Up {
        get { return _up; }
        set { _up = value; }
    }

    private bool _down;
    public bool Down {
        get { return _down; }
        set { _down = value; }
    }

    private bool _right;
    public bool Right {
        get { return _right; }
        set { _right = value; }
    }

    private bool _left;
    public bool Left {
        get { return _left; }
        set { _left = value; }
    }
}


/// <summary>
/// MAIN CLASS
/// </summary>
public class ControllableObject : MonoBehaviour {

    // Events are like broadcasting a message. Other scripts which are looking for specific events
    // will have a reference to it so they can take appropriate action with a function of their own.
    // Please ask directly ask me if this does not make sense.
    public delegate void ControllableObjectEvent(Buttons button);
    public static event ControllableObjectEvent OnButton;
    public static event ControllableObjectEvent OnButtonDown;
    public static event ControllableObjectEvent OnButtonUp;


    // Class variables
    private Facing _facingDirection = Facing.Right;
    public Facing FacingDirection {
        get { return _facingDirection; }
        set { _facingDirection = value; }
    }

    // Keeps all the possible aiming directions. 
    private short[] _aimDirections = new short[8];

    private int _currentKey;
    public int CurrentKey {
        get { return _currentKey; }
    }
    private int _aimDirection;
    public int AimDirection {
        get { return _aimDirection; }
    }

    // Other Action scripts will used this dictionary to query a button's state.
    private Dictionary<Buttons, ButtonState> buttonStates = new Dictionary<Buttons, ButtonState>();

    private void Start() {

        short angle = 0;
        for (byte i = 0; i < 8; ++i) {
            _aimDirections[i] = angle;
            angle += 45;
        }
    }

    public void SetAimDirection(int key) {
        _currentKey = key;
        _aimDirection = _aimDirections[_currentKey];
    }


    // Updated each of the Dictionary's Buttons and their State. Depending on whether the button has initially
    // been pressed, is being held down, or has just been release, this function will call the 
    // appropriate Event function so other scritps can take necessary actions.
    public void SetButtonState(Buttons button, bool isPressed) {

        // Create a new button and state if it is not already in the dictionary
        if (!buttonStates.ContainsKey(button)) {
            buttonStates.Add(button, new ButtonState());
        }

        // When the input has ceased
        if (buttonStates[button].IsPressed && !isPressed) {
            buttonStates[button].PressTime = 0.0f;
            if (OnButtonUp != null) {
                OnButtonUp(button);
            }
        }
        else if (buttonStates[button].IsPressed && isPressed) {

            // When the input has initially begun
            if (buttonStates[button].PressTime == 0.0f) {
                if (OnButtonDown != null) {
                    OnButtonDown(button);
                }
            }
            // When the input is continuous
            if (buttonStates[button].PressTime >= 0.0f) {
                if (OnButton != null) {
                    OnButton(button);
                }
            }
            buttonStates[button].PressTime += Time.deltaTime;
        }
        // Update the status of the input
        buttonStates[button].IsPressed = isPressed;
    }

    public bool GetButtonPress(Buttons button) {
        if (buttonStates.ContainsKey(button)) {
            return buttonStates[button].IsPressed;
        }
        else {
            return false;
        }
    }

    //// Return the time the button has been pressed.
    public float GetButtonPressTime(Buttons button) {
        if (buttonStates.ContainsKey(button)) {
            return buttonStates[button].PressTime;
        }
        else {
            return 0.0f;
        }
    }

    private short GetAimDirectionAngle(byte key) {
        return _aimDirections[key];
    }

    private IEnumerator StunDelay() {
        yield return new WaitForSeconds(0.25f);
    }
}
