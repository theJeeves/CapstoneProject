using UnityEngine;
using UnityEngine.Events;

public class InstructionText : MonoBehaviour {

    #region Private Fields
    [SerializeField]
    private string _ds4Instructions;
    [SerializeField]
    private string _xboxInstructions;

    [SerializeField]
    private bool _enableInput = false;

    private XFloat m_DisplayTime = 3.0f;

    #endregion Private Fields

    #region Events
    public static event UnityAction<string> DisplayHint;
    public static event UnityAction HideHint;

    #endregion Events

    #region Private Methods
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.IsNotPlayer()) { return; }

        if (m_DisplayTime != null)
        {
            bool isDS4 = InputManager.Instance.GetComponent<InputManager>().controllerType == 0 ? true : false;

            DisplayHint?.Invoke(isDS4 ? _ds4Instructions : _xboxInstructions);

            if (_enableInput)
            {
                InputManager.Instance.GetComponent<InputManager>().StartInput();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.IsNotPlayer()) { return; }

        if (m_DisplayTime != null && m_DisplayTime.IsExpired)
        {
            m_DisplayTime = null;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.IsNotPlayer()) { return; }

        HideHint?.Invoke();

        if (m_DisplayTime != null)
        {
            m_DisplayTime.Reset();
        }
    }

    #endregion Private Methods
}
