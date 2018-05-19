using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class EndLevelWindow : GenericWindow
{
    #region Constants
    private const string SAVE_FILE_PATH = "ScriptableObjects/PlayerSaveFile";
    private const string STATS_NUMBERS = "STATS NUMBERS";

    #endregion Constants

    #region Fields
    [SerializeField]
    private GameObject _continueButton;
    [SerializeField]
    private GameObject _backToMainButton;

    private SOSaveFile m_SOSaveHandler;
    private Text[] m_Stats;

    private int m_CurrentLevel = 0;

    #endregion Fields

    protected override void OnEnable() {
        base.OnEnable();

        m_CurrentLevel = SceneManager.GetActiveScene().buildIndex;

        m_SOSaveHandler = Resources.Load(SAVE_FILE_PATH, typeof(SOSaveFile)) as SOSaveFile;

        m_Stats = GameObject.Find(STATS_NUMBERS).GetComponentsInChildren<Text>();

        m_Stats[0].text = m_SOSaveHandler.CurrentDeathCount.ToString();
        m_Stats[1].text = (m_SOSaveHandler.InProgressJouleShots + m_SOSaveHandler.InProgressPersuaderShots).ToString();

        if (m_CurrentLevel == 3) {
            _continueButton.SetActive(false);
            firstSelected = _backToMainButton;
        }
        else {
            _continueButton.SetActive(true);
            firstSelected = _continueButton;
        }
    }

    private void Update() {
        switch (m_CurrentLevel) {
            case 1:
                m_Stats[2].text = FormatTime(m_SOSaveHandler.CurrentLevel1Time);
                m_Stats[3].text = FormatTime(m_SOSaveHandler.BestLevel1Time);
                break;
            case 2:
                m_Stats[2].text = FormatTime(m_SOSaveHandler.CurrentLevel2Time);
                m_Stats[3].text = FormatTime(m_SOSaveHandler.BestLevel2Time);
                break;
            case 3:
                m_Stats[2].text = FormatTime(m_SOSaveHandler.CurrentLevel3Time);
                m_Stats[3].text = FormatTime(m_SOSaveHandler.BestLevel3Time);
                break;
        }
    }

    #region Events
    public static event GenericWindowEvent OnContinue;
    public static event GenericWindowEvent OnBackToMain;

    #endregion Events

    #region Public Methods
    public void ContinueButton()
    {
        OnContinue?.Invoke(WindowIDs.EndOfLevelWindow, WindowIDs.None);
    }

    public void BackToMainButton()
    {
        OnBackToMain?.Invoke(WindowIDs.EndOfLevelWindow, WindowIDs.StartWindow);
    }

    #endregion Public Methods

    #region Private Methods
    private string FormatTime(float value)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(value);
        return string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
    }

    #endregion Private Methods
}
