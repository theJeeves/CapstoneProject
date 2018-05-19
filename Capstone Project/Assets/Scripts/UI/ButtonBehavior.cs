using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonBehavior : MonoBehaviour, ISelectHandler, IDeselectHandler/*, IPointerEnterHandler, IPointerExitHandler*/ {

    #region Fields
    [SerializeField]
    private Color _normalColor = Color.black;
    [SerializeField]
    private Color _highlightedColor = Color.cyan;

    private Text m_Text;

    #endregion Fields

    #region Initilaizers
    private void OnEnable() {
        m_Text = GetComponentInChildren<Text>();
    }

    #endregion Initializers


    #region Finalizers
    private void OnDisable() {
        m_Text.color = _normalColor;
    }

    #endregion Finalizers

    #region Public Methods
    public void OnSelect(BaseEventData eventData) {
        m_Text.color = _highlightedColor;
    }

    public void OnDeselect(BaseEventData eventData) {
        m_Text.color = _normalColor;
    }

    public void HighlightButton() {

        if (m_Text == null) { m_Text = GetComponentInChildren<Text>(); }
        m_Text.color = _highlightedColor;
    }

    #endregion Public Methods

    //public void OnPointerEnter(PointerEventData data) {
    //    _text.color = Color.cyan;
    //}

    //public void OnPointerExit(PointerEventData data) {
    //    _text.color = Color.black;
    //}
}
