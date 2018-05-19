using UnityEngine;

public class EnableDisableSmartCamera : MonoBehaviour
{
    #region Private Fields
    [SerializeField]
    private bool _oneTimeUse = false;

    [Space]

    [Header("Smart Camera X")]
    [SerializeField]
    private bool _XEnableOnEnter = false;
    [SerializeField]
    private bool _XDisableOnEnter = false;
    [SerializeField]
    private bool _XEnableOnExit = false;
    [SerializeField]
    private bool _XDisableOnExit = false;

    [Space]
    [Header("Smart Camera Y")]
    [SerializeField]
    private bool _YEnableOnEnter = false;
    [SerializeField]
    private bool _YDisableOnEnter = false;
    [SerializeField]
    private bool _YEnableOnExit = false;
    [SerializeField]
    private bool _YDisableOnExit = false;

    private GameObject _camera;       // Reference to the main Camera

    #endregion Private Fields


    #region Private Initializers
    private void OnEnable()
    {
        _camera = GameObject.FindGameObjectWithTag(Tags.SmartCameraTag);
    }

    #endregion Private Initializers

    #region Private Methods
    private void OnTriggerEnter2D(Collider2D otherGO)
    {

        if (otherGO?.gameObject.tag == Tags.PlayerTag)
        {
            if (_XDisableOnEnter) {
                _camera.GetComponent<SmartCameraXPosition>().enabled = false;
            }
            else if (_XEnableOnEnter) {
                _camera.GetComponent<SmartCameraXPosition>().enabled = true;
            }

            if (_YDisableOnEnter) {
                _camera.GetComponent<SmartCameraYPosition>().enabled = false;
            }
            else if (_YEnableOnEnter) {
                _camera.GetComponent<SmartCameraYPosition>().enabled = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D otherGO)
    {

        if (otherGO?.gameObject.tag == Tags.PlayerTag)
        {
            if (_XEnableOnExit) {
                _camera.GetComponent<SmartCameraXPosition>().enabled = true;
            }
            else if (_XDisableOnExit) {
                _camera.GetComponent<SmartCameraXPosition>().enabled = false;
            }

            if (_YEnableOnExit) {
                _camera.GetComponent<SmartCameraYPosition>().enabled = true;
            }
            else if (_YDisableOnExit) {
                _camera.GetComponent<SmartCameraYPosition>().enabled = false;
            }

            if (_oneTimeUse) {
                GetComponent<BoxCollider2D>().enabled = false;
            }
        }
    }

    #endregion Private Methods
}
