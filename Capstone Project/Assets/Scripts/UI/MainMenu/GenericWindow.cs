using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class GenericWindow : MonoBehaviour {

    [SerializeField]
    protected GameObject firstSelected;
    protected EventSystem eventSystem {
        get { return GameObject.Find("EventSystem").GetComponent<EventSystem>(); }
    }

    protected virtual void Awake() {
        Close();
    }

    public virtual void OnFocus() {
        eventSystem.SetSelectedGameObject(firstSelected);
    }

    protected virtual void Display(bool value) {
        gameObject.SetActive(value);
    }

    public virtual void Open() {
        Display(true);
        OnFocus();
    }

    public virtual void Close() {
        Display(false);
    }
}
