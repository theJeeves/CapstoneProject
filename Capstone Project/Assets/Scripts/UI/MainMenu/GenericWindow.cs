﻿using UnityEngine;
using UnityEngine.EventSystems;

public enum WindowIDs {
    StartWindow = 0,
    LevelSelectWindow = 1,
    StatsWindow = 2,
    CreditsWindow = 3,
    EndOfLevelWindow = 4,
    PauseWindow = 5,
    QuitWindow = 6,
    None = -1
}

public class GenericWindow : MonoBehaviour {

    public delegate void GenericWindowEvent(WindowIDs close, WindowIDs open);

    [SerializeField]
    protected GameObject firstSelected;

    protected GameManager _GM;
    protected WindowManager _WM;
    protected EventSystem _ES;

    protected virtual void OnEnable() {
        _GM = GameManager.Instance.GetComponent<GameManager>();
        _WM = WindowManager.Instance.GetComponent<WindowManager>();
        _ES = EventSystemSingleton.Instance.GetComponent<EventSystem>();
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
