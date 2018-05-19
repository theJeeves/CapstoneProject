using UnityEngine;
using UnityEngine.UI;

public abstract class ReloadAnimation : MonoBehaviour {

    #region Protected Fields
    protected Image _ammoImage;

    #endregion Protected Fields

    #region Private Fields
    private float m_Timer = 0.0f;
    private bool m_CanAnimate;
    private bool m_PlayAudio;

    #endregion Private Fields

    #region Initializers
    protected virtual void Awake() {
        _ammoImage = GetComponent<Image>();
        m_CanAnimate = false;
    }

    #endregion Initializers

    #region Finalizers
    protected virtual void OnDisable() {

        if (_ammoImage.fillAmount < 1.0f) {
            _ammoImage.fillAmount = 1.0f;
        }
    }

    #endregion Finalizers

    #region Protected Methods
    protected virtual void Update() {

        if (m_CanAnimate && _ammoImage.fillAmount < 1) {
            _ammoImage.fillAmount += 1.0f / m_Timer * Time.deltaTime;
        }
        else if (m_CanAnimate && _ammoImage.fillAmount >= 1) {
            m_CanAnimate = false;
            if (m_PlayAudio) {
                m_PlayAudio = false;
                GetComponent<AudioSource>().Play();
            }
        }
    }

    protected virtual void Reload(float reloadTime) {

        m_PlayAudio = true;
        _ammoImage.fillAmount = 0;
        m_Timer = reloadTime;
        m_CanAnimate = true;
    }

    protected virtual void ZeroFillAmount() {
        _ammoImage.fillAmount = 0;
    }

    protected virtual void DisplayAmmo() {
        _ammoImage.fillAmount = 1;
    }

    #endregion Protected Methods
}
