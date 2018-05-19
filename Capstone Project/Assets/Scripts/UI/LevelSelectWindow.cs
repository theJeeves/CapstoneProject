public class LevelSelectWindow : GenericWindow {

    #region Events
    public static event GenericWindowEvent OnBackButton;

    #endregion Events

    #region Public Methods
    public void BackButton()
    {
        OnBackButton?.Invoke(WindowIDs.LevelSelectWindow, WindowIDs.StartWindow);
    }

    #endregion Public Methods
}
