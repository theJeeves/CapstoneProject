public class CreditsWindow : GenericWindow {

    public static event GenericWindowEvent OnBackButton;

    public void BackButton() {
        if (OnBackButton != null) { OnBackButton(WindowIDs.CreditsWindow, WindowIDs.StartWindow); }
    }
}
