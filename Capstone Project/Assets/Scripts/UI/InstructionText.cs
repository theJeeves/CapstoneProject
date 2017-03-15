using UnityEngine;
using System.Collections;

public class InstructionText : MonoBehaviour {

    public delegate void Hint_ControlsEvent1(string hints);
    public delegate void Hint_ControlsEvent2();
    public static event Hint_ControlsEvent1 DisplayHint;
    public static event Hint_ControlsEvent2 HideHint;

    [SerializeField]
    private string _ds4Instructions;
    [SerializeField]
    private string _xboxInstructions;

    [SerializeField]
    private bool _enableInput = false;

    private bool _isDirty = false;
    private float _displayTime = 3.0f;
    private float _timer = 0.0f;
    private bool _displayed = false;

    private bool _ds4;


    private void Update() {

        if (_timer != 0.0f && Time.time - _timer > _displayTime) {
            _displayed = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D player) {

        if (!_isDirty && player.gameObject.tag == "Player" && DisplayHint != null) {

            _timer = Time.time;

            _ds4 = InputManager.Instance.GetComponent<InputManager>().controllerType == 0 ? true : false;
            if (_ds4) {
                DisplayHint(_ds4Instructions);
            }
            else {
                DisplayHint(_xboxInstructions);
            }

            if (_enableInput) { InputManager.Instance.GetComponent<InputManager>().StartInput(); }
        }
    }

    private void OnTriggerExit2D(Collider2D player) {

        if (!_isDirty && player.gameObject.tag == "Player" && HideHint != null) {

            HideHint();
            if (_displayed) _isDirty = true;
            else _timer = 0.0f;
        }
    }
}
