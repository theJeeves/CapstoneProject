using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public enum WindowIDs {
    StartWindow = 0,
    LevelSelectWindow = 1,
    StatsWindow = 2,
    CreditsWindow = 3,
    QuitWindow = 4,
    None = -1
}

public class GenericWindow : MonoBehaviour {

    [SerializeField]
    protected GameObject firstSelected;

    protected GameManager _GM;
    protected WindowManager _WM;
    protected EventSystem _ES;

    protected virtual void OnEnable() {
        _GM = GameManager.Instance;
        _WM = WindowManager.Instance;
        _ES = GameObject.Find("EventSystem").GetComponent<EventSystem>();
    }

    public virtual void Select() {
        _ES.SetSelectedGameObject(firstSelected);
    }

    public virtual void Deselect() {
        _ES.SetSelectedGameObject(null);
    }

    protected virtual void Display(bool value) {
        gameObject.SetActive(value);
    }

    public virtual void Open() {
        Display(true);
        Select();
    }

    public virtual void Close() {
        Display(false);
    }
}
