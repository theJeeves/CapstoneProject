public class PauseWindow : GenericWindow {

    #region Events
    public static event GenericWindowEvent OnContinueButton;
    public static event GenericWindowEvent OnResartLevelButton;
    public static event GenericWindowEvent OnBackToMainButton;

    #endregion Events

    #region Initializers
    protected override void OnEnable() {
        base.OnEnable();
        firstSelected.GetComponent<ButtonBehavior>().HighlightButton();
    }

    #endregion Initializers

    #region Public Methods
    public void ContinueButton()
    {
        OnContinueButton?.Invoke(WindowIDs.PauseWindow, WindowIDs.None);
    }

    public void ResartLevelButton()
    {
        OnResartLevelButton?.Invoke(WindowIDs.PauseWindow, WindowIDs.None);
    }

    public void BackToMainButton()
    {
        OnBackToMainButton?.Invoke(WindowIDs.PauseWindow, WindowIDs.StartWindow);
    }

    #endregion Public Methods
}
