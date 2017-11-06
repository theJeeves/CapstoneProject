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

    private float m_defaultDisplayTime = 3.0f;
    [SerializeField]
    private float m_displayTime = 3.0f;
    private bool m_inTrigger = false;

    private bool _ds4;

    private void Start() {
        m_displayTime = m_defaultDisplayTime;
    }

    private void Update() {

        if (m_inTrigger && m_displayTime != float.NegativeInfinity) {
            TimeTools.TimeExpired(ref m_displayTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D player) {

        m_inTrigger = true;

        if (m_displayTime != float.NegativeInfinity && player.gameObject.tag == "Player" && DisplayHint != null) {

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

        m_inTrigger = false;

        if (m_displayTime != float.NegativeInfinity && player.gameObject.tag == "Player" && HideHint != null) {

            HideHint();

            m_displayTime = m_displayTime <= 0.0f ? float.NegativeInfinity : m_defaultDisplayTime;
        }
    }
}
