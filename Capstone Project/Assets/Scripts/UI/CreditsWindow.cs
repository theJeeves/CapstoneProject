public class CreditsWindow : GenericWindow {

    #region Events
    public static event GenericWindowEvent OnBackButton;

    #endregion Events

    #region Public Methods
    public void BackButton()
    {
        OnBackButton?.Invoke(WindowIDs.CreditsWindow, WindowIDs.StartWindow);
    }

    #endregion Public Methods
}
