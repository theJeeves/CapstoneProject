using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class LevelSelectButtons : GenericWindow {

    #region Private Fields
    [SerializeField]
    private Button[] _levelButtons;

    private Selectable m_BackButton;
    private bool m_ButtonSet = false;

    #endregion Private Fields

    #region Finalizer
    private void OnDisable()
    {
        m_ButtonSet = false;
        m_BackButton = null;
    }

    #endregion Finalizer

    #region Events
    public static event GenericWindowEvent ToggleWindows;
    public static event UnityAction<int> SelectLevel;

    #endregion Events

    #region Public Methods
    /// <summary>
    /// Indicates which level was selected and triggers the event for the level to be loaded.
    /// </summary>
    /// <param name="level"></param>
    public void LevelSelected(int level)
    {
        SelectLevel?.Invoke(level);
        CloseLevelSelectWindow();
    }

    #endregion Public Methods

    #region Private Methods
    private void Update()
    {
        if (m_BackButton == null)
        {
            m_BackButton = GameObject.FindGameObjectWithTag(Tags.BackButtonTag)?.GetComponent<Selectable>();
        }

        if (m_BackButton != null && !m_ButtonSet)
        {
            for (int i = 0; i < _levelButtons.Length; ++i)
            {
                Navigation navigation = _levelButtons[i].navigation;
                navigation.selectOnLeft = m_BackButton;
                navigation.selectOnRight = m_BackButton;

                _levelButtons[i].navigation = navigation;
            }
            m_ButtonSet = true;
        }
    }

    private void CloseLevelSelectWindow()
    {
        ToggleWindows?.Invoke(WindowIDs.LevelSelectWindow, WindowIDs.None);
    }

    #endregion Private Methods
}