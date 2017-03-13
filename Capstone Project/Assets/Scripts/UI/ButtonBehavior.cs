using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonBehavior : MonoBehaviour, ISelectHandler, IDeselectHandler/*, IPointerEnterHandler, IPointerExitHandler*/ {

    [SerializeField]
    private Color _normalColor = Color.black;
    [SerializeField]
    private Color _highlightedColor = Color.cyan;

    private Text _text;

    private void OnEnable() {
        _text = GetComponentInChildren<Text>();
    }

    private void OnDisable() {
        _text.color = _normalColor;
    }

    public void OnSelect(BaseEventData eventData) {
        _text.color = _highlightedColor;
    }

    public void OnDeselect(BaseEventData eventData) {
        _text.color = _normalColor;
    }

    public void HighlightButton() {

        if (_text == null) { _text = GetComponentInChildren<Text>(); }
        _text.color = _highlightedColor;
    }

    //public void OnPointerEnter(PointerEventData data) {
    //    _text.color = Color.cyan;
    //}

    //public void OnPointerExit(PointerEventData data) {
    //    _text.color = Color.black;
    //}
}
