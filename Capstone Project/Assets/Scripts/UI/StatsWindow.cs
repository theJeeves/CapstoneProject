using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class StatsWindow : GenericWindow {

    public static event GenericWindowEvent OnBack;
    public static event GenericWindowEvent OnResetStats;

    private SOSaveFile _SOSaveHandler;
    private Text[] _stats;

    protected override void OnEnable() {
        base.OnEnable();

        _SOSaveHandler = Resources.Load("ScriptableObjects/PlayerSaveFile", typeof(SOSaveFile)) as SOSaveFile;

        _stats = GameObject.Find("StatsTextBox 2").GetComponentsInChildren<Text>();

        //_stats[0].text = _SOSaveHandler.DeathCount.ToString();
        //_stats[1].text = _SOSaveHandler.TotalShotsFired.ToString();
        //_stats[2].text = _SOSaveHandler.PersuaderShots.ToString();
        //_stats[3].text = _SOSaveHandler.JouleShots.ToString();
        //_stats[4].text = _SOSaveHandler.TotalEnemiesKilled.ToString();
        //_stats[5].text = FormatTime(_SOSaveHandler.TotalTimePlayed);
        //_stats[6].text = FormatTime(_SOSaveHandler.BestLevel1Time);
        //_stats[7].text = FormatTime(_SOSaveHandler.BestLevel2Time);
        //_stats[8].text = FormatTime(_SOSaveHandler.BestLevel3Time);

        //_stats[5].text = _SOSaveHandler.AcidVectorsKilled.ToString();
        //_stats[6].text = _SOSaveHandler.ExplosiveVectorsKilled.ToString();
        //_stats[7].text = _SOSaveHandler.FlyingVectorsKilled.ToString();
        //_stats[8].text = _SOSaveHandler.SnipersKilled.ToString();
        //_stats[9].text = _SOSaveHandler.ChargersKilled.ToString();
    }

    private void Update() {
        _stats[0].text = _SOSaveHandler.DeathCount.ToString();
        _stats[1].text = _SOSaveHandler.TotalShotsFired.ToString();
        _stats[2].text = _SOSaveHandler.PersuaderShots.ToString();
        _stats[3].text = _SOSaveHandler.JouleShots.ToString();
        _stats[4].text = _SOSaveHandler.TotalEnemiesKilled.ToString();
        _stats[5].text = FormatTime(_SOSaveHandler.TotalTimePlayed);
        _stats[6].text = FormatTime(_SOSaveHandler.BestLevel1Time);
        _stats[7].text = FormatTime(_SOSaveHandler.BestLevel2Time);
        _stats[8].text = FormatTime(_SOSaveHandler.BestLevel3Time);
    }

    public void ResetStats() {
        if (OnResetStats != null) { OnResetStats(WindowIDs.None, WindowIDs.None); }
    }

    public void Back() {
        if (OnBack != null) { OnBack(WindowIDs.StatsWindow, WindowIDs.StartWindow); }
    }

    string FormatTime(float value) {
        TimeSpan timeSpan = TimeSpan.FromSeconds(value);
        return string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
    }
}
