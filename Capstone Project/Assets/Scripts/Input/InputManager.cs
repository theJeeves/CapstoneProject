using UnityEngine;
using System.Collections;

public enum Buttons {
    MoveRight,
    MoveLeft,
}

public enum Condition {
    GreaterThan,
    LessThan
}

[System.Serializable]
public class InputAxisState {
    [SerializeField]
    private string axisName;

    [SerializeField]
    private Buttons _button;
    public Buttons Button {
        get { return _button; }
    }

    [SerializeField]
    private float offValue;

    [SerializeField]
    private Condition condition;

    public bool IsPressed {
        get {
            float isPressed = Input.GetAxis(axisName);
            switch (condition) {
                case Condition.GreaterThan:
                    return isPressed > offValue;
                case Condition.LessThan:
                    return isPressed < offValue;
            }

            return false;
        }
    }
}

public class InputManager : MonoBehaviour {

    [SerializeField]
    private ControllableObject character;
    [SerializeField]
    private InputAxisState[] inputs;
	
	// Update is called once per frame
	void Update () {
	    
        foreach(InputAxisState input in inputs) {
            character.SetButtonState(input.Button, input.IsPressed);
        }
	}
}
