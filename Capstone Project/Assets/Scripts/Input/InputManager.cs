using UnityEngine;
using System.Collections;

/// <summary>
/// This script allows a developer to define which buttons will be relavant to each Controllable GameObject.
/// It processes each of the defined inputs by quering against Unity's input manager for the Axes we have asked for.
/// This is done in class InputAxisState. This InputAxisState is used to check against the ButtonState found in each
/// ControllableObject's Dictonary.
/// </summary>

// Contains all the buttons which can/will be used in the game
public enum Buttons {
    MoveRight,
    MoveLeft,
    Shoot,
    WeaponSwap,
    AimUp,
    AimDown,
    AimLeft,
    AimRight,
    Reload
}

// This enum is used to determine if our code should start reading input from the player.
// Example: if the player has moved the right analog stick slightly to the right, this will
// be greater than the OffValue and we should process the input
public enum Condition {
    GreaterThanOffValue,
    LessThanOffValue
}

// This class acts is an abstraction of Unity's own input manager. By using this abstraction,
// we can easily change which buttons are associated with different Axes and change their properties.
// This is much easier than hardcoding each action and then configuring Unity's input settings
// to change the properties for each axis. This will become more apparent if/when we do co-op.
[System.Serializable]
public class InputAxisState {

    [SerializeField]
    private string _axisName;                //Which axis will this input be looking for
    [SerializeField]
    private string _altAxisName;            // If the axis has an alternative button.

    [SerializeField]
    private Buttons _button;                // Which button will be mapped to the assigned axis
    public Buttons Button {
        get { return _button; }
    }

    [SerializeField]
    private float _offValue;                 // When should it stop reading input from the player

    [SerializeField]
    private Condition _condition;            // When should it start reading input from the player

    // Used to determine when our code should start/stop reading input on the defined axis based on Condition and OffValue
    public bool IsPressed {
        get {
            float isPressed = 0.0f;

            if (_altAxisName != "") {
                isPressed = Input.GetAxis(_altAxisName);
            }
            if (isPressed == 0.0f){
                 isPressed = Input.GetAxis(_axisName);
            }

            switch (_condition) {
                case Condition.GreaterThanOffValue:
                    return isPressed > _offValue;
                case Condition.LessThanOffValue:
                    return isPressed < _offValue;
                default:
                    break;
            }

            return false;
        }
    }
}

public class InputManager : MonoBehaviour {

    [SerializeField]
    private ControllableObject _player;             //Which GameObject we will be processing input for

    [Space]
    [Header("DUALSHOCK INPUTS")]
    [SerializeField]
    private InputAxisState[] _DSInputs;               // An array of all the inputs which can be used by the player

    [Space]
    [Header("XBOX INPUTS")]
    [SerializeField]
    private InputAxisState[] _XBOXInputs;

    private int controllerType = -1;

    private void Start() {
        // Get a list of all availabe gamepads
        string[] inputs = Input.GetJoystickNames();

        // Determine if the player is using a Dualshock or Xbox controller
        // Stop looking after the first one is found.
        foreach(string input in inputs) {
            if (input != "") {
                controllerType = input == "Wireless Controller" ? 0 : 1;
                break;
            }
        }
    }

    // Update is called once per frame
    void Update() {

        // Depending on the controller type, run through all the inputs and check if any of the
        // button states has changed.
        if (controllerType == 0) {
            foreach (InputAxisState input in _DSInputs) {
                _player.SetButtonState(input.Button, input.IsPressed);
            }
        }
        else {
            foreach (InputAxisState input in _XBOXInputs) {
                _player.SetButtonState(input.Button, input.IsPressed);
            }
        }
    }
}
