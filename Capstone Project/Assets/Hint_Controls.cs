using UnityEngine;
using System.Collections;

public class Hint_Controls : MonoBehaviour {

    public delegate void Hint_ControlsEvent1(string hints);
    public delegate void Hint_ControlsEvent2();
    public static event Hint_ControlsEvent1 DisplayHint;
    public static event Hint_ControlsEvent2 HideHint;

    [SerializeField]
    private string _hint;
    [SerializeField]
    private bool _rightTrigger = false;

    private Vector3 _boxPos;

    private void OnEnable() {
        _boxPos.x = _rightTrigger ? transform.position.x + GetComponent<BoxCollider2D>().size.x / 2 :
            transform.position.x - GetComponent<BoxCollider2D>().size.x / 2;
    }

    private void OnTriggerEnter2D(Collider2D player) {
    
        if (player.gameObject.tag == "Player" && DisplayHint != null) {
            switch (_rightTrigger) {
                case true:

                    if (player.transform.position.x >= _boxPos.x) {
                        DisplayHint(_hint);
                    }
                    break;

                case false:

                    if (player.transform.position.x <= _boxPos.x) {
                        DisplayHint(_hint);
                    }
                    break;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D player) {

        if (player.gameObject.tag == "Player" && HideHint != null) {
            switch (_rightTrigger) {
                case true:
                    if (player.transform.position.x > _boxPos.x || player.transform.position.y < _boxPos.y) {
                        HideHint();
                    }
                    break;

                case false:
                    if (player.transform.position.x < _boxPos.x) {
                        HideHint();
                    }
                    break;
            }
        }
    }
}
