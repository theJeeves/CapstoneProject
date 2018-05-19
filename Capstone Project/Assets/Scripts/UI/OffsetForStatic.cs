using UnityEngine;
using UnityEngine.UI;

public class OffsetForStatic : MonoBehaviour {

    #region Fields
    [SerializeField]
    private float _offsetSpeed = 0.0f;

    private Image m_Image;

    #endregion Fields

    #region Initializers
    private void OnEnable() {
        m_Image = GetComponent<Image>();
        m_Image.material.mainTextureOffset = Vector2.zero;
    }

    #endregion Initializers

    #region Private Methods
    private void Update()
    {
        m_Image.material.mainTextureOffset = new Vector2(0.0f, (m_Image.material.mainTextureOffset.y + _offsetSpeed));
    }

    #endregion Private Methods
}
