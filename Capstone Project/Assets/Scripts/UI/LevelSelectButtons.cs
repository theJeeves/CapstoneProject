using UnityEngine;
using UnityEngine.UI;
using System;

public class LevelSelectButtons : GenericWindow {

    #region Private Fields
    [SerializeField]
    private Button[] _levelButtons;

    private Selectable _backButton;
    private bool _buttonSet = false;

    #endregion Private Fields

    #region Finalizer
    private void OnDisable() {
        _buttonSet = false;
        _backButton = null;
    }

    #endregion Finalizer

    #region Events
    public static event GenericWindowEvent ToggleWindows;
    public static event EventHandler<int> SelectLevel;

    #endregion Events

    #region Public Methods
    /// <summary>
    /// Indicates which level was selected and triggers the event for the level to be loaded.
    /// </summary>
    /// <param name="level"></param>
    public void LevelSelected(int level)
    {
        SelectLevel?.Invoke(this, level);
        CloseLevelSelectWindow();
    }

    #endregion Public Methods

    #region Private Methods
    private void Update()
    {
        if (_backButton == null)
        {
            _backButton = GameObject.FindGameObjectWithTag(StringConstantUtility.BACK_BUTTON)?.GetComponent<Selectable>();
        }

        if (!_buttonSet && _backButton != null)
        {
            for (int i = 0; i < _levelButtons.Length; ++i)
            {

                Navigation navigation = _levelButtons[i].navigation;
                navigation.selectOnLeft = _backButton;
                navigation.selectOnRight = _backButton;

                _levelButtons[i].navigation = navigation;
            }
            _buttonSet = true;
        }
    }

    private void CloseLevelSelectWindow()
    {
        ToggleWindows?.Invoke(WindowIDs.LevelSelectWindow, WindowIDs.None);
    }

    #endregion Private Methods
}