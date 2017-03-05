using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StatsWindow : GenericWindow {

    public delegate void StatsWindowEvent(WindowIDs close, WindowIDs open);
    public static event StatsWindowEvent OnBack;

    public void Back() {
        if (OnBack != null) { OnBack(WindowIDs.StatsWindow, WindowIDs.StartWindow); }
    }
}
