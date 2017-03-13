using UnityEngine;
using System.Collections;

public class CallTextHUD : MonoBehaviour {

    [SerializeField]
    private float _dropDownSpeed = 0.0f;

    private bool _display = false;
    private RectTransform _transform;

	private void OnEnable() {
        InstructionText.DisplayHint += DisplayHint;
        InstructionText.HideHint += HideHint;

        _transform = GetComponent<RectTransform>();
    }

    private void OnDisable() {
        InstructionText.DisplayHint -= DisplayHint;
        InstructionText.HideHint -= HideHint;
    }

    private void Update() {
        
        if (_display && _transform.anchoredPosition.y > -145.0f) {
            _transform.anchoredPosition = Vector3.MoveTowards(_transform.anchoredPosition, new Vector3(0.0f, -145.0f, 0), Time.deltaTime * _dropDownSpeed);
        }
        else if (!_display && _transform.anchoredPosition.y < 125.0f) {
            _transform.anchoredPosition = Vector3.MoveTowards(_transform.anchoredPosition, new Vector3(0.0f, 125.0f, 0), Time.deltaTime * _dropDownSpeed);
        }
    }

    private void DisplayHint(string hint) {
        _display = true;
    }

    private void HideHint() {
        _display = false;
    }
}
