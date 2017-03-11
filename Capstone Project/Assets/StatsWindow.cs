using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StatsWindow : GenericWindow {

    public static event GenericWindowEvent OnBack;

    private SOSaveFile _SOSaveHandler;
    private Text[] _stats;

    protected override void OnEnable() {
        base.OnEnable();

        _SOSaveHandler = Resources.Load("ScriptableObjects/PlayerSaveFile", typeof(SOSaveFile)) as SOSaveFile;

        _stats = GameObject.Find("StatsTextBox 2").GetComponentsInChildren<Text>();

        _stats[0].text = _SOSaveHandler.DeathCount.ToString();
        _stats[1].text = _SOSaveHandler.TotalShotsFired.ToString();
        _stats[2].text = _SOSaveHandler.PersuaderShots.ToString();
        _stats[3].text = _SOSaveHandler.JouleShots.ToString();
        _stats[4].text = _SOSaveHandler.TotalEnemiesKilled.ToString();
        _stats[5].text = _SOSaveHandler.AcidVectorsKilled.ToString();
        _stats[6].text = _SOSaveHandler.ExplosiveVectorsKilled.ToString();
        _stats[7].text = _SOSaveHandler.FlyingVectorsKilled.ToString();
        _stats[8].text = _SOSaveHandler.SnipersKilled.ToString();
        _stats[9].text = _SOSaveHandler.ChargersKilled.ToString();
    }

    public void Back() {
        OnBack(WindowIDs.StatsWindow, WindowIDs.StartWindow);
    }
}
