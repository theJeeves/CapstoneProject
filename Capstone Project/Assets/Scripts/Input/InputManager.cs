using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

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

public class InputManager : Singleton<InputManager> {

    [SerializeField]
    private ControllableObject _player;             //Which GameObject we will be processing input for
    [SerializeField]
    private SOInputType _DS4Inputs;
    [SerializeField]
    private SOInputType _XBOXInputs;

    public int controllerType = -1;
    public bool _canTakeInput = true;
    private bool _limitedTime = true;
    private float _pauseDuration = 0.0f;
    private float _timer = 0.0f;

    protected override void Awake() {

        _DS4Inputs = Resources.Load("Inputs/DualShock 4", typeof(SOInputType)) as SOInputType;
        _XBOXInputs = Resources.Load("Inputs/Xbox One", typeof(SOInputType)) as SOInputType;

        _canTakeInput = Application.loadedLevel == 0 ? false : true;
        _limitedTime = _canTakeInput;

        base.Awake();
    }


    private void Start() {
        // Get a list of all availabe gamepads
        string[] inputs = Input.GetJoystickNames();

        // Determine if the player is using a Dualshock or Xbox controller
        // Stop looking after the first one is found.
        foreach(string input in inputs) {
            if (input != "") {
                controllerType = input == "Wireless Controller" || input == "PLAYSTATION(R)3 Controller" ? 0 : 1;
                break;
            }
        }
    }

    // Update is called once per frame
    private void FixedUpdate() {

        if (_player == null && GameObject.FindGameObjectWithTag("Player") != null) {
            _player = GameObject.FindGameObjectWithTag("Player").GetComponent<ControllableObject>();
        }

        if (!_canTakeInput && _limitedTime) {
            _timer = _timer > 0.0f ? _timer : Time.time;

            if (Time.time - _timer > _pauseDuration) {
                _canTakeInput = true;
                _timer = 0.0f;
            }
        }

        if (_player != null) {
            // Depending on the controller type, run through all the inputs and check if any of the
            // button states has changed.
            if (controllerType == 0) {
                foreach (InputAxisState input in _DS4Inputs.inputs) {
                    if (_canTakeInput) {
                        _player.SetButtonState(input.Button, input.IsPressed);
                    }
                    else {
                        _player.SetButtonState(input.Button, false);
                    }
                }
            }
            else {
                foreach (InputAxisState input in _XBOXInputs.inputs) {
                    if (_canTakeInput) {
                        _player.SetButtonState(input.Button, input.IsPressed);
                    }
                    else {
                        _player.SetButtonState(input.Button, false);
                    }
                }
            }
        }
    }

    public void AssignPlayer(GameObject player) {
        _player = player.GetComponent<ControllableObject>();
        _canTakeInput = true;
        _limitedTime = true;
    }

    public void PauseInput(float pauseDuration) {
        _pauseDuration = pauseDuration;
        _limitedTime = true;
        _canTakeInput = false;
    }

    public void StopInput() {
        _canTakeInput = false;
        _limitedTime = false;
    }

    public void StartInput() {
        _canTakeInput = true;
        _limitedTime = true;
    }

    public void OnLevelWasLoaded(int level) {
        if (level > 0) {
            _player = null;
        }
    }
}
