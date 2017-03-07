using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndLevelWindow : GenericWindow {

    public delegate void EndLevelWindowEvent(WindowIDs close, WindowIDs open);
    public static event EndLevelWindowEvent OnBack;

    private SOSaveFile _SOSaveHandler;
    private Text[] _stats;

    protected override void OnEnable() {
        base.OnEnable();

        _SOSaveHandler = Resources.Load("ScriptableObjects/PlayerSaveFile", typeof(SOSaveFile)) as SOSaveFile;

        _stats = GameObject.Find("STATS NUMBERS").GetComponentsInChildren<Text>();

        _stats[0].text = _SOSaveHandler.CurrentDeathCount.ToString();
        _stats[1].text = _SOSaveHandler.CurrentShotsFired.ToString();
        //_stats[2].text = _SOSaveHandler.PersuaderShots.ToString();
        //_stats[3].text = _SOSaveHandler.JouleShots.ToString();
    }
}
